﻿using Hospital.Api.QueueManagement.DTO.Shared;

namespace Hospital.Api.QueueManagement.DTO.Doctor
{
    public class Get_AvailableDoctors_Request:RequestBase
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public int HoursTakes { get; set; }
    }
}
