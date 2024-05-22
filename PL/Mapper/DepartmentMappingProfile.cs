using AutoMapper;
using DAL.Entities;
using PL.Models;

namespace PL.Mapper
{
    public class DepartmentMappingProfile : Profile
    {
        public DepartmentMappingProfile()
        {
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
