using Hospital.Domain.DoctorEntity;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Add_WorkingHour_Request
    {
        public class Add_WorkingHours_Request
        {
            public Guid DoctorId { get; set; }
            public List<WorkingHourDTO> WorkingHours { get; set; }
        }

    }
   public class WorkingHourDTO
    {
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
    }
}
