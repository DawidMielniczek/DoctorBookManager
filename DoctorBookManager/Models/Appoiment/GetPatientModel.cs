namespace DoctorBookManager.Models.Appoiment
{
    public class GetPatientModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime VisitTime { get; set; }
    }
}
