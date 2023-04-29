using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Add_Doctor_Request
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CapacityPerDay { get; set; }
        public List<WorkingHourDTO> WorkingHours { get; set; }
    }


    public class WorkingHourDTO
    {
        public DayOfWeekEnum DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Capacity { get; set; }
    }
}
