namespace DoctorBookManager.Models.Appoiment
{
    public class GetAvailableHours
    {
        public int DoctId { get; set; }
        public List<int> AvaialbleHours{ get; set; }
        public DateTime Date { get; set; }
        public string DoctorName { get; set; }
        //public int Hour { get; set; }
    }
}
