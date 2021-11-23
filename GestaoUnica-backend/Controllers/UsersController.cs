using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoUnica_backend.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using GestaoUnica_backend.Services.Models;
using GestaoUnica_backend.Services.Interfaces;
using System;

namespace GestaoUnica_backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserBusiness _userBusiness;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogService _logService;
        private readonly IRoleBusiness _roleBusiness;

        public UsersController(IUserBusiness userBusiness, IPasswordHasher passwordHasher, IRoleBusiness roleBusiness, ILogService logService)
        {
            _userBusiness = userBusiness;
            _passwordHasher = passwordHasher;
            _logService = logService;
            _roleBusiness = roleBusiness;
        }

        // GET: api/Users
        [Authorize(Roles = "SuperAdmin,Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> FindAll()
        {
            try
            {
                return _userBusiness.FindAll();
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Users/FindAll",                 //Url
                        "Listar todos os usuarios",                 //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/Users/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> FindByID(int id)
        {
            try
            {
                var user = _userBusiness.FindByID(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Users/FindByID",                //Url
                        "Obter usuario por Id",                     //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Users/FindByUserName
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> FindByUsername([FromQuery] User user)
        {
            try
            {
                var foundUser = _userBusiness.FindByUsername(user.UserName);

                if (foundUser == null)
                {
                    return NotFound();
                }

                return foundUser;
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Users/FindByUsername",          //Url
                        "Obter usuario por UserName",               //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "SuperAdmin,Administrador")]
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> AlterUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            try
            {
                var userBase = _userBusiness.FindByID(id);

                user.DataInclusao = userBase.DataInclusao;
                user.IdUserInclusao = userBase.IdUserInclusao;
                user.Password = user.Password != userBase.Password && user.Password.Length < 75 ? _passwordHasher.Hash(user.Password) : userBase.Password;
                user.UserName = userBase.UserName;
                user.NumTentativasAcesso = userBase.IsActive != user.IsActive ? 0 : userBase.NumTentativasAcesso;
                user.DataAlteracao = DateTime.Now;
                user.IdUserAlteracao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;

                return _userBusiness.Update(user);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Users/AlterUser",               //Url
                        "Editar um usuario",                        //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        [Authorize]
        [HttpPatch("{id}")]
        public async Task<ActionResult> ChangePassword(int id, [FromBody] User user)
        {
            if(id != user.Id)
            {
                return BadRequest(new { message = "Ids não conferem" });
            }

            var hash = _passwordHasher.Hash(user.Password);

            try
            {
                var userAdmin = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString());
                _userBusiness.ChangePassword(id, hash, userAdmin.Id);

                return Ok(new { message = "Senha alterada com sucesso" });
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Users/ChangePassword",          //Url
                        "Alteracao de senha",                       //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );
            return BadRequest(new { message = "Ocorreu um erro ao tentar alterar a senha" });
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "SuperAdmin,Administrador")]
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Password))
                    user.Password = _passwordHasher.Hash(user.Password);
                else
                    return BadRequest(new { message = "Senha inicial de cadastro não informada." });
                
                user.DataInclusao = DateTime.Now;
                user.IdUserInclusao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;

                return _userBusiness.Create(user);
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Users/CreateUser",              //Url
                        "Criar um usuario",                         //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        /* Não será possível excluir um usuario para não comprometer os logs
        // DELETE: api/Users/5
        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = _userBusiness.FindByID(id);
                if (user == null)
                {
                    return NotFound();
                }

                _userBusiness.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Login/Authenticate",            //Url
                        "Tentativa de autenticação no sistema",     //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }*/

        private bool UserExists(int id)
        {
            return _userBusiness.Exists(id);
        }
    }
}
