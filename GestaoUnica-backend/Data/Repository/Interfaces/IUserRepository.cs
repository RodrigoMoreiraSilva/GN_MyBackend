using GestaoUnica_backend.Services.Models;

namespace GestaoUnica_backend.Data.Repository.Interfaces
{
    public interface IUserRepository
    {
        User FindByUsername(string username);
        bool ChangePassword(int userId,string password, int idAdmin);
        void UpdateTentativasAcesso(User user, int num);
    }
}
