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
    public class PatientRepository : Repository<Patient>, IPatientRepository
    {
        public PatientRepository(DbContext context) : base(context)
        {
        }
    }
}
