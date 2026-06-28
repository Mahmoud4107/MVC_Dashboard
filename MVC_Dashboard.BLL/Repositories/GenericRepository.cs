using Microsoft.EntityFrameworkCore;
using MVC_Dashboard.BLL.Interfaces;
using MVC_Dashboard.DAL.Data;
using MVC_Dashboard.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Dashboard.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : ModelBase
    {
        private protected readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
                return (IEnumerable<T>)_context.Employees.Include(E => E.Department).AsNoTracking().ToList();
            else
                return _context.Set<T>().AsNoTracking().ToList();
        }
            
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }
        public void Delete(T entity)
        {
            _context.Remove(entity);
        }
        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
