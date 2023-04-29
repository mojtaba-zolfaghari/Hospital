using Hospital.Shared.Shared;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Domain.DoctorEntity
{
    public class WorkingHour:BaseEntity
    {
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
        public Guid DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; }
    }
}