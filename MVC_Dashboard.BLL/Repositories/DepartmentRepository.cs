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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext Context;
        public DepartmentRepository(ApplicationDbContext _Context)
        {
            Context = _Context;
        }
        public int Add(Department department)
        {
            Context.Add(department);
            return Context.SaveChanges();
        }

        public int Delete(Department department)
        {
            Context.Remove(department);
            return Context.SaveChanges();
        }
        public int Update(Department department)
        { 
            Context.Update(department);
            return Context.SaveChanges();
        }

        public IEnumerable<Department> GetAll()
        {
            return Context.Departments.AsNoTracking().ToList();
        }

        public Department GetById(int Id)
        {
            ///var department = Context.Departments.Local.Where(D => D.Id == Id).FirstOrDefault();
            ///if (department == null) {
            ///    department = Context.Departments.Where(D => D.Id == Id).FirstOrDefault();

            //return Context.Departments.Find(Id);
            return Context.Find<Department>(Id); ;
        }

    }
}
