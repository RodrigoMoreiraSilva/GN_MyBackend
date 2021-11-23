using GestaoUnica_backend.Services.Models;

namespace GestaoUnica_backend.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string RefreshToken(string token);
        User DecodeToken(string token);
    }
}
