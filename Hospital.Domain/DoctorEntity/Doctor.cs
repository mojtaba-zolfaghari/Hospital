using Hospital.Domain.QueueEntity;
using Hospital.Shared.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Domain.DoctorEntity
{
    public class Doctor : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CapacityPerDay { get; set; }
        public bool IsAvailable { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
