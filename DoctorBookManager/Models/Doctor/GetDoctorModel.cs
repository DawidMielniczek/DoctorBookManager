using System.ComponentModel.DataAnnotations;

namespace DoctorBookManager.Models.Doctor
{
    public class GetDoctorModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }


        [MaxLength(35)]
        public string Specialization { get; set; }
        [Required]
        public int StartWorkHour { get; set; }
        [Required]
        public int EndWorkHour { get; set; }
        [MaxLength(35)]
        public string FullName { get; set; }

        [MaxLength(25)]
        public string City { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
