using Hospital.Shared.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Interfaces.Doctor
{
    public interface IPatientRepository:IRepository<Domain.QueueEntity.Patient>
    {
    }
}
