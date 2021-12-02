using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Business.Implementation
{
    public class FileBusiness : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;
        private readonly IConfiguration _config;

        public FileBusiness(IHttpContextAccessor context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
            _basePath = Directory.GetCurrentDirectory() + _config.GetSection("FileDestination").Value;
        }

        public byte[] GetFile(string filename)
        {
            var filePath = _basePath + filename;
            return File.ReadAllBytes(filePath);
        }

        public bool DeleteFile(string filename)
        {
            try
            {
                var filePath = _basePath + filename;
                File.Delete(filePath);
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<BaseFileDetail> SaveFileToDisk(IFormFile file) 
        {

            var fileDetail = new BaseFileDetail();

            var fileType = Path.GetExtension(file.FileName);
            var baseUrl = _context.HttpContext.Request.Host;

            if(fileType.ToLower().Equals(".pdf") || fileType.ToLower().Equals(".jpg") || fileType.ToLower().Equals(".png") || fileType.ToLower().Equals(".jpeg"))
            {
                var docName = Path.GetFileName(file.FileName);

                if(file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetail.DocumentName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocUrl = Path.Combine(baseUrl + "/api/File/downloadFile/" + fileDetail.DocumentName);

                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<BaseFileDetail>> SaveFilesToDisk(IList<IFormFile> files)
        {
            var list = new List<BaseFileDetail>();

            foreach(var file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }
            return list;
        }


    }
}
