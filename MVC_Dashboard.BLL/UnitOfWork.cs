using MVC_Dashboard.BLL.Interfaces;
using MVC_Dashboard.BLL.Repositories;
using MVC_Dashboard.DAL.Data;
using MVC_Dashboard.DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Dashboard.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        Hashtable Repo;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Repo = new Hashtable();
        }

        public IGenericRepository<T> Repository<T>() where T : ModelBase
        {

            // T => Employee
            ////

            var Key = typeof(T).Name;  // Employee

            if(!Repo.ContainsKey(Key))
            {
                if (Key == nameof(Employee))
                {
                    var Repository = new EmployeeRepository(_context);
                    Repo.Add(Key, Repository);
                }
                else
                {
                    var Repository = new GenericRepository<T>(_context);
                    Repo.Add(Key, Repository);
                }

            }
            return Repo[Key] as IGenericRepository<T>;
        }

        public int Complete()
          => _context.SaveChanges();
    }
}
