using Hospital.Shared.Shared;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Domain.QueueEntity
{
    public class Patient : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string NationalCode { get; set; }
        public DateTime Birthdate { get; set; }
        public string PhoneNumber { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
