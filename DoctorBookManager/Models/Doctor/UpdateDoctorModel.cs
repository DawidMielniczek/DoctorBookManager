using System.ComponentModel.DataAnnotations;

namespace DoctorBookManager.Models.Doctor
{
    public class UpdateDoctorModel
    {
        public int Id { get; set; }


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
    }
}
