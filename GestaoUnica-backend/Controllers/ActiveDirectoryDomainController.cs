using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GestaoUnica_backend.Services.Models;
using GestaoUnica_backend.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using GestaoUnica_backend.Services.Interfaces;

namespace GestaoUnica_backend.Controllers
{
    [Authorize(Roles = "SuperAdmin,Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveDirectoryDomainController : ControllerBase
    {
        private readonly IActiveDirectoryBusiness _activeDirectoryBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IRoleBusiness _roleBusiness;
        private readonly ILogService _logService;

        public ActiveDirectoryDomainController(IActiveDirectoryBusiness activeDirectoryBusiness, IUserBusiness userBusiness, IRoleBusiness roleBusiness, ILogService logService)
        {
            _activeDirectoryBusiness = activeDirectoryBusiness;
            _userBusiness = userBusiness;
            _roleBusiness = roleBusiness;
            _logService = logService;
        }

        // GET: api/ActiveDirectoryDomains
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActiveDirectoryDomain>>> FindAll()
        {
            try
            {
                return _activeDirectoryBusiness.FindAll();
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/ActiveDirectoryController/FindAll", //Url
                        "Listar todos os domínios",                     //Acao
                        _roleBusiness.FindByRoleName(logUser.Role),     //Regra
                        logUser,                                        //Usuario
                        ex.Message                                      //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/ActiveDirectoryDomains/5   
        [HttpGet("{id}")]
        public async Task<ActionResult<ActiveDirectoryDomain>> FindById(int id)
        {
            try
            {
                var activeDirectoryDomain = _activeDirectoryBusiness.FindByID(id);

                if (activeDirectoryDomain == null)
                {
                    return NotFound();
                }

                return activeDirectoryDomain;
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/ActiveDirectoryController/FindAll", //Url
                        "Buscar um domínio por ID",                     //Acao
                        _roleBusiness.FindByRoleName(logUser.Role),     //Regra
                        logUser,                                        //Usuario
                        ex.Message                                      //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/ActiveDirectoryDomains/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<ActiveDirectoryDomain>> Update(int id, ActiveDirectoryDomain activeDirectoryDomain)
        {
            try
            {
                if (id != activeDirectoryDomain.Id)
                {
                    return BadRequest();
                }

                activeDirectoryDomain.DataAlteracao = DateTime.Now;
                activeDirectoryDomain.IdUserAlteracao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;

                return _activeDirectoryBusiness.Update(activeDirectoryDomain);
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/ActiveDirectoryController/Update",  //Url
                        "Alterar um domínio",                           //Acao
                        _roleBusiness.FindByRoleName(logUser.Role),     //Regra
                        logUser,                                        //Usuario
                        ex.Message                                      //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/ActiveDirectoryDomains
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754    
        [HttpPost]
        public async Task<ActionResult<ActiveDirectoryDomain>> Create(ActiveDirectoryDomain activeDirectoryDomain)
        {
            try
            {
                activeDirectoryDomain.DataInclusao = DateTime.Now;
                activeDirectoryDomain.IdUserInclusao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;
                return _activeDirectoryBusiness.Create(activeDirectoryDomain);
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/ActiveDirectoryController/Create",  //Url
                        "Cadastrar um novo domínio",                    //Acao
                        _roleBusiness.FindByRoleName(logUser.Role),     //Regra
                        logUser,                                        //Usuario
                        ex.Message                                      //Erro
                    );

                return BadRequest(new { message = ex.Message });
            }
        }

        //// DELETE: api/ActiveDirectoryDomains/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteActiveDirectoryDomain(int id)
        //{
        //    var activeDirectoryDomain = await _context.DominiosActiveDirectory.FindAsync(id);
        //    if (activeDirectoryDomain == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DominiosActiveDirectory.Remove(activeDirectoryDomain);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool ActiveDirectoryDomainExists(int id)
        //{
        //    return _context.DominiosActiveDirectory.Any(e => e.Id == id);
        //}
    }
}
