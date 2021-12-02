using GestaoUnica_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GestaoUnica_backend.Business.Interfaces
{
    public interface IDemotech_ServicoBusiness
    {
        Demotech_Servico Create(Demotech_Servico demotech_Servico);
        Demotech_Servico FindByID(int id);
        List<Demotech_Servico> FindAll();
        Demotech_Servico Update(Demotech_Servico demotech_Servico);
    }
}
