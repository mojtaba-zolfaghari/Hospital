using Hospital.Api.QueueManagement.DTO.Shared;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Get_AvailableDoctors_Request:RequestBase
    {
        public DateTime Date { get; set; }
    }
}
