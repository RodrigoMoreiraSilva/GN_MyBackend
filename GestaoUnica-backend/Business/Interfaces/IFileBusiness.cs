using GestaoUnica_backend.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Business.Interfaces
{
    public interface IFileBusiness
    {
        public byte[] GetFile(string filename);
        public bool DeleteFile(string filename);
        public Task<BaseFileDetail> SaveFileToDisk(IFormFile file);
        public Task<List<BaseFileDetail>> SaveFilesToDisk(IList<IFormFile> files);
    }
}
