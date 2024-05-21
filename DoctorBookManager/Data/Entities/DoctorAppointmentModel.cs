using DoctorBookManager.Data.EntityBase;
using Microsoft.Build.Framework;

namespace DoctorBookManager.Data.Entities
{
    public class DoctorAppointmentModel:IEntity, IHasCreationTime
    {
        public int Id { get; set; }
        [Required]
        public string PatientId { get; set; }
        [Required]
        public int DoctorId { get; set; }
        public DoctorModel Doctor { get; set; }
        public DateTime CreationTime { get ; set ; }
        [Required]
        public DateTime VisitTime { get; set; }
    }
}
