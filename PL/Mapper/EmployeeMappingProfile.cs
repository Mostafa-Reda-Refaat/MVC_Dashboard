using AutoMapper;
using DAL.Entities;
using PL.Models;

namespace PL.Mapper
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            //CreateMap<EmployeeViewModel, Employee>();
            //CreateMap<Employee, EmployeeViewModel>();

            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

        }
    }
}
