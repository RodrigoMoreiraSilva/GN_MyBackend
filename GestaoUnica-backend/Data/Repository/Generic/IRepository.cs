using GestaoUnica_backend.Models.Base;
using System.Collections.Generic;

namespace GestaoUnica_backend.Repository.Generic
{
    public interface IRepository<T> where T : BaseEntity
    {
        T Create(T item);
        T FindByID(int id);
        List<T> FindAll();
        T Update(T item);
        void Delete(int id);
        bool Exists(int id);
    }
}
