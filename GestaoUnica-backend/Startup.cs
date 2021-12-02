using GestaoUnica_backend.Business.Implementation;
using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Context;
using GestaoUnica_backend.Data.Repository.Implementations;
using GestaoUnica_backend.Data.Repository.Interfaces;
using GestaoUnica_backend.Repository.Generic;
using GestaoUnica_backend.Services.Implementation;
using GestaoUnica_backend.Services.Interfaces;
using GestaoUnica_backend.Services.Sealeds;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace GestaoUnica_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(x =>
            {
                x.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins(
                                            "http://localhost:4200"
                                            //"http://gestaounica.dev.intranet.gesto.com.br"
                                            //"http://gestaounica.qa.intranet.gesto.com.br"
                                            //"https://www.gestaounica.gesto.com.br"
                                            )
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestaoUnica_backend", Version = "v1" });
            });
            #region ConnectionString
            services.AddDbContext<SQLContext>(x => x.UseSqlServer(Configuration.GetConnectionString("Connection")));
            #endregion

            #region Token e Active Directory
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("TokenConfig:Secret").Value.ToString());
            
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
                         
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IActiveDirectoryService, ActiveDirectoryService>();
            #endregion
            
            #region Repositorios
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            #endregion

            #region Business - Regras de negócio
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IRoleBusiness, RoleBusiness>();
            services.AddScoped<IActiveDirectoryBusiness, ActiveDirectoryBusiness>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IFileBusiness, FileBusiness>();
            services.AddScoped<IDemotech_ServicoBusiness, Demotech_ServicoBusiness>();
            #endregion

            #region Services
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GestaoUnica_backend v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowOrigin"); //Deve ficar apos UseHttpsRedirection e UseRouting e antes de UseEndpoints

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
