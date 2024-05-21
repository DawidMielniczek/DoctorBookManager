namespace DoctorBookManager.Models
{
    public class GetDoctorReviewModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Grade { get; set; }
        public DateTime CreationDate { get; set; }

        public string Content { get; set; }
        public int IdReview { get; set; }
    }
}
