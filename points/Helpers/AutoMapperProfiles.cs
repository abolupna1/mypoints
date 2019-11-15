using AutoMapper;
using points.Models;
using points.ModelViews.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace points.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Employee, EmployeeCreateViewModel>().ReverseMap();
            CreateMap<Employee, EmployeeEditViewModel>().ReverseMap();



        }
    }
}
