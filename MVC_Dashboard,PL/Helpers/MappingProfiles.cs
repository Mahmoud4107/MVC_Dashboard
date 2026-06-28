using AutoMapper;
using MVC_Dashboard.DAL.Models;
using MVC_Dashboard_PL.ViewModels;

namespace MVC_Dashboard_PL.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
