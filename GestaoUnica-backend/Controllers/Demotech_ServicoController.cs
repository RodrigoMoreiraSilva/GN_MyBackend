using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoUnica_backend.Models;
using GestaoUnica_backend.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using GestaoUnica_backend.Models.Base;

namespace GestaoUnica_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Demotech_ServicoController : ControllerBase
    {
        private readonly IDemotech_ServicoBusiness _demotech_ServicoBusiness;
        private readonly IFileBusiness _fileBusiness;
        private readonly IUserBusiness _userBusiness;

        public Demotech_ServicoController(IDemotech_ServicoBusiness demotech_ServicoBusiness, IFileBusiness fileBusiness, IUserBusiness userBusiness)
        {
            _demotech_ServicoBusiness = demotech_ServicoBusiness;
            _fileBusiness = fileBusiness;
            _userBusiness = userBusiness;
        }

        // GET: api/Demotech_Servico
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Demotech_Servico>>> GetDemotech_Servico()
        {
            return _demotech_ServicoBusiness.FindAll();
        }

        // GET: api/Demotech_Servico/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Demotech_Servico>> GetDemotech_Servico(int id)
        {
            var demotech_Servico = _demotech_ServicoBusiness.FindByID(id);

            if (demotech_Servico == null)
            {
                return NotFound();
            }

            return demotech_Servico;
        }

        // PUT: api/Demotech_Servico/5
        [Authorize(Roles = "SuperAdmin,Administrador,Demotech-Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Demotech_Servico>> PutDemotech_Servico(int id)
        {
            Demotech_Servico demotech_Servico = JsonConvert.DeserializeObject<Demotech_Servico>(Request.Form["demotech_Servico"]);
            var file = Request.Form.Files.GetFile("file");

            if (id != demotech_Servico.Id)
            {
                return BadRequest();
            }

            try
            {
                var fileDetail = new BaseFileDetail();

                var oldFile = _demotech_ServicoBusiness.FindByID(id).DocumentName;

                if (!string.IsNullOrEmpty(oldFile) && file != null)
                {
                    _fileBusiness.DeleteFile(oldFile);
                    fileDetail = await _fileBusiness.SaveFileToDisk(file);

                    demotech_Servico.DocumentName = fileDetail.DocumentName;
                    demotech_Servico.DocType = fileDetail.DocType;
                    demotech_Servico.DocUrl = fileDetail.DocUrl;
                }
                               
                demotech_Servico.DataAlteracao = DateTime.Now;
                demotech_Servico.IdUserAlteracao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;

                return _demotech_ServicoBusiness.Update(demotech_Servico);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Demotech_ServicoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/Demotech_Servico
        [Authorize(Roles = "SuperAdmin,Administrador,Demotech-Admin")]
        [HttpPost]
        public async Task<ActionResult<Demotech_Servico>> PostDemotech_Servico()
        {
            Demotech_Servico demotech_Servico = JsonConvert.DeserializeObject<Demotech_Servico>(Request.Form["demotech_Servico"]);
            var file = Request.Form.Files.GetFile("file");

            var fileDetail = await _fileBusiness.SaveFileToDisk(file);

            demotech_Servico.DocumentName = fileDetail.DocumentName;
            demotech_Servico.DocType = fileDetail.DocType;
            demotech_Servico.DocUrl = fileDetail.DocUrl;
            demotech_Servico.DataInclusao = DateTime.Now;
            demotech_Servico.IdUserInclusao = _userBusiness.FindByToken(HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString()).Id;

            return _demotech_ServicoBusiness.Create(demotech_Servico);
        }

        // DELETE: api/Demotech_Servico/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteDemotech_Servico(int id)
        //{
        //    var demotech_Servico = _demotech_ServicoBusiness.FindByID(id);
        //    if (demotech_Servico == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Demotech_Servico.Remove(demotech_Servico);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool Demotech_ServicoExists(int id)
        {
            var servico = _demotech_ServicoBusiness.FindByID(id);

            if (servico != null)
                return true;
            else
                return false;
        }
    }
}
