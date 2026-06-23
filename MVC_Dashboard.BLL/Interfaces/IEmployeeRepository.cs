using MVC_Dashboard.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Dashboard.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public Employee GetEmployeeByAddress(string address);
        public IQueryable<Employee> SearchByName(string Name);

    }
}
