using GestaoUnica_backend.Business.Interfaces;
using GestaoUnica_backend.Data.Repository.Interfaces;
using GestaoUnica_backend.Repository.Generic;
using GestaoUnica_backend.Services.Interfaces;
using GestaoUnica_backend.Services.Models;
using System.Collections.Generic;

namespace GestaoUnica_backend.Business.Implementation
{
    public class UserBusiness : IUserBusiness
    {
        private readonly IRepository<User> _repository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserBusiness(IRepository<User> repository, IUserRepository userRepository, ITokenService tokenService)
        {
            _repository = repository;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public bool ChangePassword(int id, string password, int idAdmin)
        {
            return _userRepository.ChangePassword(id, password, idAdmin);
        }

        public User Create(User user)
        {
            return _repository.Create(user);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }

        public bool Exists(int id)
        {
            return _repository.Exists(id);
        }

        public List<User> FindAll()
        {
            return _repository.FindAll();
        }

        public User FindByID(int id)
        {
            return _repository.FindByID(id);
        }

        public User FindByToken(string token)
        {
             var userClaims = _tokenService.DecodeToken(token);
           
            if (userClaims != null)
                return FindByUsername(userClaims.UserName);
            else
                return new User() { Id = -1 };
        }

        public User FindByUsername(string username)
        {
            return _userRepository.FindByUsername(username);
        }

        public User Update(User user)
        {
            return _repository.Update(user);
        }

        public void UpdateTentativaAcesso(User user, int num)
        {
            _userRepository.UpdateTentativasAcesso(user, num);
        }
    }
}
