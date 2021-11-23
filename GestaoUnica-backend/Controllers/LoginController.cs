using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Services.Interfaces;
using GestaoUnica_backend.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly IActiveDirectoryService _activeDirectoryService;
        private readonly IUserBusiness _userBusiness;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleBusiness _roleBusiness;
        private readonly ILogService _logService;

        public LoginController(IConfiguration configuration, ITokenService tokenService, IActiveDirectoryService activeDirectoryService, IUserBusiness userBusiness,
                               IPasswordHasher passwordHasher, IRoleBusiness roleBusiness, ILogService logService)
        {
            _config = configuration;
            _tokenService = tokenService;
            _activeDirectoryService = activeDirectoryService;
            _userBusiness = userBusiness;
            _passwordHasher = passwordHasher;
            _roleBusiness = roleBusiness;
            _logService = logService;
        }

        [HttpPost]
        [Route("token")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] User model)
        {
            var user = _userBusiness.FindByUsername(model.UserName);
            try
            {
                if (user != null)
                {
                    model.Id = user.Id;
                    model.Role = user.Role;
                    model.DataInclusao = user.DataInclusao;
                    model.IdUserInclusao = user.IdUserInclusao;
                    model.DataAlteracao = user.DataAlteracao;
                    model.IdUserAlteracao = user.IdUserAlteracao;
                    model.IsActive = user.IsActive;
                    model.ActiveDirectoryAuth = user.ActiveDirectoryAuth;
                    model.PasswordExpired = user.PasswordExpired;
                }
                else
                {
                    return NotFound(new { message = "Usuário não encontrado" });
                }

                if (!model.IsActive)
                    return Unauthorized(new { message = "Usuário bloqueado / inativo" });

                bool authenticated;
                if (model.ActiveDirectoryAuth)
                {
                    if (!string.IsNullOrEmpty(model.LdapSection))
                        authenticated = await _activeDirectoryService.IsADAuthenticated(model);
                    else
                        return BadRequest(new { message = "Domínio de autenticação não informado" });
                }
                else
                {
                    authenticated = _passwordHasher.Check(user.Password, model.Password).Verified;
                }

                if (!authenticated)
                {
                    if (user.NumTentativasAcesso < Convert.ToInt32(_config.GetSection("LimiteTentativasAcesso").Value))
                    {
                        _userBusiness.UpdateTentativaAcesso(user, user.NumTentativasAcesso + 1);

                        return Unauthorized(new { message = "Usuário ou senha inválidos" });
                    }
                    else
                    {
                        user.IsActive = false;
                        _userBusiness.Update(user);

                        return Unauthorized(new { message = "Conta bloqueada, procure um administrador do sistema para reativar seu usuario" });
                    }
                }

                if(user.NumTentativasAcesso > 0)
                    _userBusiness.UpdateTentativaAcesso(user, 0);
                
                var token = _tokenService.GenerateToken(model);

                model.Password = string.Empty;

                return Ok(new
                {
                    user = model,
                    token = token
                });
            }
            catch (Exception ex)
            {
                _logService.SalvarLog
                    (
                        "Controller/Login/Authenticate",        //Url
                        "Tentativa de autenticação no sistema", //Acao
                        _roleBusiness.FindByRoleName(user.Role),//Regra
                        user,                                   //Usuario
                        ex.Message                              //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("refreshtoken")]
        [Authorize]
        public async Task<IActionResult> RefreshToken()
        {
            var token = string.Empty;
            try
            {
                token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                string newToken = _tokenService.RefreshToken(token);

                return Ok(new
                {
                    token = newToken
                });
            }
            catch (Exception ex)
            {
                var logUser = _userBusiness.FindByToken(token);
                _logService.SalvarLog
                    (
                        "Controller/Login/RefreshToken",            //Url
                        "Tentativa de refresh token",               //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
