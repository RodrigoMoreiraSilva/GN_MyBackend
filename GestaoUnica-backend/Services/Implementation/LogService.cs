using GestaoUnica_backend.Repository.Generic;
using GestaoUnica_backend.Services.Interfaces;
using GestaoUnica_backend.Services.Models;
using System;

namespace GestaoUnica_backend.Services.Implementation
{
    public class LogService : ILogService
    {
        private readonly IRepository<Log> _repository;

        public LogService(IRepository<Log> repository)
        {
            _repository = repository;
        }
        public void SalvarLog(string url, string acao, Role regra = null, User user = null, string erro = "")
        {
            var log = new Log()
            {
                Url = url,
                AcaoRealizada = acao,
                Regra = regra,
                IdUserInclusao = user != null ? user.Id : -1,
                Observacao = erro,
                DataInclusao = DateTime.Now
            };

            _repository.Create(log);
        }
    }
}
