using Hospital.Application.Interfaces.Appointment;
using Hospital.Application.Interfaces.Auth;
using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Implementation.Appointment
{
    internal class AppointmentRepository : Repository<Domain.QueueEntity.Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbContext context) : base(context)
        {
        }
    }
}
