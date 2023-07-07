using Hospital.Domain.DoctorEntity;
using Hospital.Shared.Shared;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Domain.QueueEntity
{
    public class Appointment : BaseEntity
    {
        public DateTime Date { get; set; }
        public bool IsConfirmed { get; set; }
        public int QueuingNumber { get; set; }
        public Guid? PatientId { get; set; }
        public virtual Patient Patient { get; set; }
        public AppointmentStatus Status { get; set; }
        public Guid DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}