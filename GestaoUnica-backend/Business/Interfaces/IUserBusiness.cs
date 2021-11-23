using GestaoUnica_backend.Services.Models;
using System.Collections.Generic;

namespace GestaoUnica_backend.Business.Interfaces
{
    public interface IUserBusiness
    {
        User Create(User user);
        User FindByID(int id);
        User FindByUsername(string username);
        User FindByToken(string token);
        List<User> FindAll();
        User Update(User user);
        void Delete(int id);
        bool Exists(int id);
        bool ChangePassword(int id, string password, int idAdmin);
        void UpdateTentativaAcesso(User user, int num);
    }
}
