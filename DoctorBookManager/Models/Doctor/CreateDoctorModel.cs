using DoctorBookManager.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace DoctorBookManager.Models.Doctor
{
    public class CreateDoctorModel
    {
        [Display(Name = "Użytkownik")]
        public string UserId { get; set; }
        
       
        [MaxLength(35)]
        [Display(Name = "Specjalizacja")]
        public string Specialization { get; set; }
        [Required]
        [Display(Name = "Godzina rozpoczęcia")]
        public int StartWorkHour { get; set; }
        [Required]       
        [Display(Name = "Godzina zakończenia")]
        public int EndWorkHour { get; set; }
        [MaxLength(35)]
        [Display(Name = "Imię Nazwisko")]
        public string FullName { get; set; }
        
        [MaxLength(25)]
        [Display(Name = "Miasto")]
        public string City { get; set; }
        public List<GetUserModel> Users { get; set; }
        public CreateDoctorModel()
        {
            Users = new List<GetUserModel>();
        }
    }
}
