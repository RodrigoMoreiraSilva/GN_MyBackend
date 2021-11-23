using GestaoUnica_backend.Context;
using GestaoUnica_backend.Data.Repository.Interfaces;
using GestaoUnica_backend.Services.Models;
using System.Linq;

namespace GestaoUnica_backend.Data.Repository.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SQLContext _context;

        public RoleRepository(SQLContext context)
        {
            _context = context;
        }
        public Role FindByRoleName(string roleName)
        {
            return _context.Regras.SingleOrDefault(x => x.Name == roleName);
        }
    }
}
