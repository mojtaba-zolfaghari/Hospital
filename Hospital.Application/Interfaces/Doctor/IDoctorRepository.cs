using Hospital.Shared.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Application.Interfaces.Doctor
{
    public interface IDoctorRepository : IRepository<Domain.DoctorEntity.Doctor>
    {
        // Add any specific methods for Doctor repository here
        Task<List<Domain.DoctorEntity.Doctor>> GetAvailableDoctors(DateTime date);
    }
}
