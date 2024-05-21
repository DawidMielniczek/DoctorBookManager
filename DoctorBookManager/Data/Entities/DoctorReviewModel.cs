using DoctorBookManager.Data.EntityBase;
using System.ComponentModel.DataAnnotations;

namespace DoctorBookManager.Data.Entities
{
    public class DoctorReviewModel : IHasCreationAudited
    {
        public int Id { get ; set ; }
        public DateTime CreationTime { get; set; }
        public string? CreatorUserId { get ; set ; }
        public int DoctorId { get; set; }
        public DoctorModel Doctor { get; set; }
        public int Grade { get; set; }
        [MaxLength(250)]
        public string? Content { get; set; }

    }
}
