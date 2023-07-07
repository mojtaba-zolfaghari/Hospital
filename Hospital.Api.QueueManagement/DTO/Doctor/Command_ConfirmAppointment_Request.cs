using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Command_ConfirmAppointment_Request
    {
        public Guid AppointmentId { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
