using Hospital.Application.Interfaces.Doctor;
using Hospital.Domain.QueueEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Implementation.Doctor
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbContext context) : base(context)
        {
        }
    }
}
