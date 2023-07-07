using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Add_Appointment_Request
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string NationalCode { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhoneNumber { get; set; }
    }
}
