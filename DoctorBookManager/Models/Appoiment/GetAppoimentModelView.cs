namespace DoctorBookManager.Models.Appoiment
{
    public class GetAppoimentModelView
    {
        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string Specialization { get; set; }
        public DateTime VisitTime { get; set; }
    }
}
