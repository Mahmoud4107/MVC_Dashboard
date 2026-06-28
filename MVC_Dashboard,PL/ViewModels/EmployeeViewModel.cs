using MVC_Dashboard.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace MVC_Dashboard_PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Max Length of name is 50 chars")]
        [MinLength(5, ErrorMessage = "Min Length of name is 5 chars")]
        public string Name { get; set; }

        [Range(22, 30)]
        public int Age { get; set; }

        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
        , ErrorMessage = "Address must be like 123-Street-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Hiring Date")]
        public DateTime HiringDate { get; set; }

        public EmpType EmployeeType { get; set; }

        public Gender Gender { get; set; }
        public int? DepartmentId { get; set; } // Foreign Key
        public Department Department { get; set; }
    }
}
