using Hospital.Application.Interfaces.Doctor;
using Hospital.Domain.DoctorEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Implementation.Doctor
{
    public class WorkingHourRepository : Repository<WorkingHour>, IWorkingHourRepository
    {
        public WorkingHourRepository(DbContext context) : base(context)
        {
        }
    }
}
