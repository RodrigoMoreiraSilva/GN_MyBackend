using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Models;
using GestaoUnica_backend.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Business.Implementation
{
    public class Demotech_ServicoBusiness : IDemotech_ServicoBusiness
    {
        private readonly IRepository<Demotech_Servico> _repository;

        public Demotech_ServicoBusiness(IRepository<Demotech_Servico> repository)
        {
            _repository = repository;
        }

        public Demotech_Servico Create(Demotech_Servico demotech_Servico)
        {
            return _repository.Create(demotech_Servico);
        }

        public List<Demotech_Servico> FindAll()
        {
            return _repository.FindAll();
        }

        public Demotech_Servico FindByID(int id)
        {
            return _repository.FindByID(id);
        }

        public Demotech_Servico Update(Demotech_Servico demotech_Servico)
        {
            return _repository.Update(demotech_Servico);
        }
    }
}
