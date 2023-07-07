using Hospital.Application.Interfaces;
using Hospital.Application.Interfaces.Doctor;
using Hospital.Infrastructure.Contexts;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Application.Implementation.Doctor
{
    public class DoctorRepository : Repository<Domain.DoctorEntity.Doctor>, IDoctorRepository
    {

        // Implement any specific methods for Doctor repository here
        public DoctorRepository(Microsoft.EntityFrameworkCore.DbContext context) : base(context)
        {
        }

        public async Task<List<Domain.DoctorEntity.Doctor>> GetAvailableDoctors(DateTime date)
        {
            var _dbContext = (Context as QueueDbContext);
            //var dayOfWeek = ConvertToDayOfWeekEnum(date.DayOfWeek);
            var availableDoctors = await _dbContext.Doctors
                .Include(d => d.Appointments)
                .ThenInclude(a => a.Patient)
                .Where(c=>c.IsAvailable)
                .ToListAsync();

            return availableDoctors;
        }

        private DayOfWeekEnum ConvertToDayOfWeekEnum(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                    return DayOfWeekEnum.Monday;
                case DayOfWeek.Tuesday:
                    return DayOfWeekEnum.Tuesday;
                case DayOfWeek.Wednesday:
                    return DayOfWeekEnum.Wednesday;
                case DayOfWeek.Thursday:
                    return DayOfWeekEnum.Thursday;
                case DayOfWeek.Friday:
                    return DayOfWeekEnum.Friday;
                case DayOfWeek.Saturday:
                    return DayOfWeekEnum.Saturday;
                case DayOfWeek.Sunday:
                    return DayOfWeekEnum.Sunday;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, null);
            }
        }
    }
}
