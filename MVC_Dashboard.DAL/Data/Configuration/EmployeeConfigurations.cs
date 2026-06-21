using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MVC_Dashboard.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_Dashboard.DAL.Data.Configuration
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(E => E.Name).IsRequired().HasColumnType("varchar").HasMaxLength(50);
            builder.Property(E => E.Address).IsRequired();
            builder.Property(E => E.Salary).HasColumnType("decimal(12,2)");

            builder.Property(E => E.Gender)
                   .HasConversion(Gender => Gender.ToString(),
                                  genderToString => (Gender)Enum.Parse(typeof(Gender), genderToString, true));

            builder.Property(E => E.EmployeeType)
                   .HasConversion(Emp => Emp.ToString(),
                                  EmpToString => (EmpType)Enum.Parse(typeof(EmpType), EmpToString, true));
        }
    }
}
