using GestaoUnica_backend.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Business.Interfaces
{
    public interface IActiveDirectoryBusiness
    {
        ActiveDirectoryDomain Create(ActiveDirectoryDomain activeDirectory);
        ActiveDirectoryDomain FindByID(int id);
        List<ActiveDirectoryDomain> FindAll();
        ActiveDirectoryDomain Update(ActiveDirectoryDomain activeDirectory);
    }
}
