using GestaoUnica_backend.Services.Models;

namespace GestaoUnica_backend.Data.Repository.Interfaces
{
    public interface IRoleRepository
    {
        Role FindByRoleName(string roleName);
    }
}
