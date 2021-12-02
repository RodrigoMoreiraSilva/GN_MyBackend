using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Models.Base;
using GestaoUnica_backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileBusiness _fileBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly ILogService _logService;
        private readonly IRoleBusiness _roleBusiness;

        public FileController(IFileBusiness fileBusiness, IUserBusiness userBusiness, ILogService logService, IRoleBusiness roleBusiness)
        {
            _fileBusiness = fileBusiness;
            _userBusiness = userBusiness;
            _logService = logService;
            _roleBusiness = roleBusiness;
        }

        [HttpGet]
        [Route("downloadFile/{fileName}")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            try
            {
                byte[] buffer = _fileBusiness.GetFile(fileName);

                if (buffer != null)
                {
                    HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                    HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                    await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
                }
                return new ContentResult();
            }
            catch (Exception ex)
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").ToString();
                var logUser = _userBusiness.FindByToken(token);

                _logService.SalvarLog
                    (
                        "Controller/File/downloadFile/{fileName}",  //Url
                        "Download de um arquivo",                   //Acao
                        _roleBusiness.FindByRoleName(logUser.Role), //Regra
                        logUser,                                    //Usuario
                        ex.Message                                  //Erro
                    );

                return new ContentResult();
            }
        }

        [HttpDelete]
        [Route("deleteFile/{fileName}")]
        public async Task<IActionResult> DeleteFileAsync(string fileName)
        {
            var deleted = _fileBusiness.DeleteFile(fileName);

            if (deleted)
            {
                return Ok(new { message = "Arquivo apagado com sucesso." });
            }
            return NotFound(new { message = "Arquivo não encontrado." });
        }

        [HttpPost]
        [Route("uploadFile")]
        [ProducesResponseType(200, Type = typeof(BaseFileDetail))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            BaseFileDetail detail = await _fileBusiness.SaveFileToDisk(file);

            return new OkObjectResult(detail);
        }

        [HttpPost]
        [Route("uploadMultipleFiles")]
        [ProducesResponseType(200, Type = typeof(List<BaseFileDetail>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadManyFiles([FromForm] List<IFormFile> files)
        {
            List<BaseFileDetail> details = await _fileBusiness.SaveFilesToDisk(files);

            return new OkObjectResult(details);
        }
    }
}
