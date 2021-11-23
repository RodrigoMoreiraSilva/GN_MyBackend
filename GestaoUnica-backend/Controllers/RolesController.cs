using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoUnica_backend.Context;
using GestaoUnica_backend.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using GestaoUnica_backend.Services.Models;
using GestaoUnica_backend.Services.Interfaces;
using System;

namespace GestaoUnica_backend.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SQLContext _context;
        private readonly IRoleBusiness _roleBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ILogService _logService;

        public RolesController(SQLContext context, IRoleBusiness roleBusiness, IUserBusiness userBusiness, ILogService logService)
        {
            _context = context;
            _roleBusiness = roleBusiness;
            _userBusiness = userBusiness;
            _logService = logService;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            try
            {
                return _roleBusiness.FindAll();
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Roles/GetRoles",                //Url
                        "Listar todas as regras",                   //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }

        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            try
            {
                var role = _roleBusiness.FindByID(id);

                if (role == null)
                {
                    return NotFound();
                }

                return role;
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Roles/GetRole",                 //Url
                        "Obter regra por id",                       //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Role>> PutRole(int id, [FromBody] Role role)
        {
            if (id != role.Id)
            {
                return BadRequest();
            }
            try
            {
                var roleBase = _roleBusiness.FindByID(id);

                role.DataInclusao = roleBase.DataInclusao;
                role.IdUserInclusao = roleBase.IdUserInclusao;
                role.Name = roleBase.Name;
                role.DataAlteracao = DateTime.Now;
                role.IdUserAlteracao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;

                return _roleBusiness.Update(role);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Roles/PutRole",                 //Url
                        "Alterar regra",                            //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                if (!RoleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return NoContent();
                }
            }
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role)
        {
            try
            {
                role.IdUserInclusao =  _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;
                role.DataInclusao = DateTime.Now;

                return _roleBusiness.Create(role);
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Roles/PostRole",                //Url
                        "Criar regra",                              //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        /*
        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = _roleBusiness.FindByID(id);
                if (role == null)
                {
                    return NotFound();
                }

                _roleBusiness.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/Roles/DeleteRole",              //Url
                        "Apagar uma regra",                         //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }*/

        private bool RoleExists(int id)
        {
            return _roleBusiness.Exists(id);
        }
    }
}
