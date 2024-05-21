using DoctorBookManager.Data.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace DoctorBookManager.Data.Entities
{
    public class DoctorModel : IHasCreationAudited
    {
        public int Id { get ; set ; }
        public string UserId { get; set; }
        public DateTime CreationTime { get ; set ; }
        public string? CreatorUserId { get ; set ; }
        [MaxLength(35)]
        public string Specialization { get; set; }
        [Required]
        public int StartWorkHour { get; set; }
        [Required]
        public int EndWorkHour { get; set; }
        [MaxLength(35)]
        public string FullName { get; set; }
        public List<DoctorAppointmentModel> DoctorAppointments { get; set; }
        public List<DoctorReviewModel> Reviews { get; set; }
        [MaxLength(25)]
        public string City { get; set; }
        public DoctorModel()
        {
            DoctorAppointments = new List<DoctorAppointmentModel>();
            Reviews = new List<DoctorReviewModel>();
        }


    }
}
