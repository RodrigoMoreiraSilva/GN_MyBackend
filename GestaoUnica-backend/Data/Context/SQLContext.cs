using GestaoUnica_backend.Models;
using GestaoUnica_backend.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GestaoUnica_backend.Context
{
    public class SQLContext: DbContext
    {
        private readonly IConfiguration _config;

        public SQLContext()
        {
            
        }
        public SQLContext(DbContextOptions<SQLContext> options, IConfiguration configuration)
            :base(options)
        {
            _config = configuration;
        }
        #region Domínios estruturais
        public DbSet<User> Usuarios { get; set; }
        public DbSet<Role> Regras { get; set; }
        public DbSet<ActiveDirectoryDomain> DominiosActiveDirectory { get; set; }
        #endregion

        #region Domínios de Negócio
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<GrupoEmpresa> GrupoEmpresa { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Demotech_Servico> Demotech_Servicos { get; set; }
        #endregion


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("Connection"));
        }
        
        public DbSet<GestaoUnica_backend.Models.Demotech_Servico> Demotech_Servico { get; set; }
    }
}
