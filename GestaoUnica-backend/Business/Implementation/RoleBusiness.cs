using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Data.Repository.Interfaces;
using GestaoUnica_backend.Repository.Generic;
using GestaoUnica_backend.Services.Models;
using System.Collections.Generic;

namespace GestaoUnica_backend.Business.Implementation
{
    public class RoleBusiness : IRoleBusiness
    {
        private readonly IRepository<Role> _repository;
        private readonly IRoleRepository _roleRepository;

        public RoleBusiness(IRepository<Role> repository, IRoleRepository roleRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
        }
        public Role Create(Role role)
        {
            return _repository.Create(role);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public bool Exists(int id)
        {
            return _repository.Exists(id);
        }

        public List<Role> FindAll()
        {
            return _repository.FindAll();
        }

        public Role FindByID(int id)
        {
            return _repository.FindByID(id);
        }

        public Role FindByRoleName(string roleName)
        {
            return _roleRepository.FindByRoleName(roleName);
        }

        public Role Update(Role role)
        {
            return _repository.Update(role);
        }
    }
}
