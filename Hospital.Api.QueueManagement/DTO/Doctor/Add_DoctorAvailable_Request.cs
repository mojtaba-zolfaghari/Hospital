namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Add_DoctorAvailable_Request
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
        public Guid DoctorId { get; set; }
    }
}
