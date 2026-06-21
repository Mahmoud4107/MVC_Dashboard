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
    public class EmployeeRepository : GenericRepository<Employee>,IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {

        }
        public Employee GetEmployeeByAddress(string address)
        {
            return _context.Employees.Where(E => E.Address.ToLower() == address.ToLower()).FirstOrDefault();
        }

    }
}
