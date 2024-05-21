using AutoMapper;
using DoctorBookManager.Data.Entities;

namespace DoctorBookManager.Models.Appoiment
{
    public class AppoimentMapProfile:Profile    
    {
        public AppoimentMapProfile()
        {
            CreateMap<DoctorAppointmentModel, GetAppoimentModelView>()
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Doctor.Specialization));
            CreateMap<DoctorAppointmentModel, GetPatientModel>();

        }
    }
}
