using GestaoUnica_backend.Services.Models;

namespace GestaoUnica_backend.Services.Interfaces
{
    public interface ILogService
    {
        void SalvarLog(string url, string acao, Role regra = null, User user = null, string erro = "");
    }
}
