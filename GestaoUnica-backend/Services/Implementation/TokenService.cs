using GestaoUnica_backend.Services.Interfaces;
using GestaoUnica_backend.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace GestaoUnica_backend.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly byte[] key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            key = Encoding.ASCII.GetBytes(_config.GetSection("TokenConfig:Secret").Value.ToString());
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(Int32.Parse(_config.GetSection("TokenConfig:Expiration").Value)), // Valor em horas
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string RefreshToken(string token)
        {
            try
            {
                var user = DecodeToken(token);

                return GenerateToken(user);

            }
            catch (Exception ex)
            {
                throw new Exception("Token inválido");
            }
        }
        public User DecodeToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecutiryToken = securityToken as JwtSecurityToken;

                if (jwtSecutiryToken == null || !jwtSecutiryToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCulture))
                    throw new SecurityTokenException("Token inválido");

                var user = new User();
                user.UserName = principal.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault().Value;
                user.Role = principal.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;

                return user;

            }catch(Exception ex)
            {
                return null;
            }
        }

       
    }
}
