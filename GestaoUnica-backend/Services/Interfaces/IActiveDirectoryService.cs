using GestaoUnica_backend.Services.Models;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Services.Interfaces
{
    public interface IActiveDirectoryService
    {
        Task<bool> IsADAuthenticated(User user);
    }
}
