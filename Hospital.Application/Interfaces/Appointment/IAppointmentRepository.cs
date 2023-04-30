using Hospital.Domain.AuthEntity;
using Hospital.Domain.QueueEntity;
using Hospital.Shared.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Interfaces.Appointment
{
    public interface IAppointmentRepository : IRepository<Domain.QueueEntity.Appointment>
    {
    }
}
