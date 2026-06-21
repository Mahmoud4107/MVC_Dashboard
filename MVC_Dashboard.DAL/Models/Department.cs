using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Dashboard.DAL.Models
{
    public class Department : ModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code Is Required Ya 7oda")]
        public int? Code { get; set; }
        public string Name { get; set; }
        [Display(Name = "Date Of Creation")]
        public DateTime DateOfCreation { get; set; }
    }
}
