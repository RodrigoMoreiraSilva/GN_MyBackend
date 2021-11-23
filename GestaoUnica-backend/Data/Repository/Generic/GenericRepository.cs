using GestaoUnica_backend.Context;
using GestaoUnica_backend.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoUnica_backend.Repository.Generic
{
    public class GenericRepository<T> : IRepository<T> where T : BaseEntity
    {
        private SQLContext _context;

        private DbSet<T> dataset;

        public GenericRepository(SQLContext context)
        {
            _context = context;
            dataset = _context.Set<T>();
        }

        public T Create(T item)
        {
            try
            {
                dataset.Add(item);
                _context.SaveChanges();
                return item;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Delete(int id)
        {
            var result = dataset.SingleOrDefault(x => x.Id.Equals(id));
            if(result != null)
            {
                try
                {
                    dataset.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        public bool Exists(int id)
        {
            return dataset.Any(x => x.Id.Equals(id));
        }

        public List<T> FindAll()
        {
            return dataset.ToList();
        }

        public T FindByID(int id)
        {
            return dataset.SingleOrDefault(x => x.Id.Equals(id));
        }

        public T Update(T item)
        {
            var result = dataset.SingleOrDefault(x => x.Id.Equals(item.Id));
            if (result != null)
            {
                try
                {
                    _context.Entry(result).CurrentValues.SetValues(item);
                    _context.SaveChanges();
                    return result;
                }
                catch (Exception)
                {

                    throw;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
