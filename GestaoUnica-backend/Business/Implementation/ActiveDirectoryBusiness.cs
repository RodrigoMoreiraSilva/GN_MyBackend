using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Repository.Generic;
using GestaoUnica_backend.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Business.Implementation
{
    public class ActiveDirectoryBusiness : IActiveDirectoryBusiness
    {
        private readonly IRepository<ActiveDirectoryDomain> _repository;

        public ActiveDirectoryBusiness(IRepository<ActiveDirectoryDomain> repository)
        {
            _repository = repository;
        }

        public ActiveDirectoryDomain Create(ActiveDirectoryDomain activeDirectory)
        {
            return _repository.Create(activeDirectory);
        }

        public List<ActiveDirectoryDomain> FindAll()
        {
            return _repository.FindAll();
        }

        public ActiveDirectoryDomain FindByID(int id)
        {
            return _repository.FindByID(id);
        }

        public ActiveDirectoryDomain Update(ActiveDirectoryDomain activeDirectory)
        {
            return _repository.Update(activeDirectory);
        }
    }
}
