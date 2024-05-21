using AutoMapper;
using DoctorBookManager.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace DoctorBookManager.Models.Doctor
{
    public class DoctorMapProfile:Profile
    {
        public DoctorMapProfile()
        {
            CreateMap<CreateDoctorModel, DoctorModel>();
            CreateMap<UpdateDoctorModel, DoctorModel>();
            CreateMap<DoctorModel, UpdateDoctorModel>();
            CreateMap<DoctorModel, GetDoctorModel>();
            CreateMap<IdentityUser, GetUserModel>();
        }
    }
}
