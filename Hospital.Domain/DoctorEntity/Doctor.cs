using Hospital.Domain.QueueEntity;
using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Hospital.Shared.Utitlities.Attributes;

namespace Hospital.Domain.DoctorEntity
{
    public class Doctor : BaseEntity
    {
        [OperatorComparer(PublicEnumes.OperatorComparer.Contains)]
        public string FirstName { get; set; }

        [OperatorComparer(PublicEnumes.OperatorComparer.Contains)]
        public string LastName { get; set; }

        [OperatorComparer(PublicEnumes.OperatorComparer.Equals)]
        public int CapacityPerDay { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<WorkingHour> WorkingHours { get; set; }
    }
}
