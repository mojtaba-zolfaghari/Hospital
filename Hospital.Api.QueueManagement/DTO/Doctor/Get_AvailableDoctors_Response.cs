using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Get_AvailableDoctors_Response
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<AvailableSlot> AvailableSlots { get; set; }
        public List<BookedSlot> BookedSlots { get; set; }
    }

    public class AvailableSlot
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Capacity { get; set; }
    }
    public class BookedSlot
    {
        public DateTime StartTime { get; set; }
        public string NationalCode { get; set; }
        public string PatientName { get; set; }
        public string PatientPhoneNumber { get; set; }
    }
}
