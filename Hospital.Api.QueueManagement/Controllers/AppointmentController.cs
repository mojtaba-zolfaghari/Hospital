using ElmahCore;
using Hospital.Api.QueueManagement.DTO;
using Hospital.Api.QueueManagement.DTO.Auth.User;
using Hospital.Api.QueueManagement.DTO.Doctor;
using Hospital.Api.QueueManagement.Utilities;
using Hospital.Application.Interfaces;
using Hospital.Domain.AuthEntity;
using Hospital.Domain.DoctorEntity;
using Hospital.Domain.QueueEntity;
using Hospital.Infrastructure.Migrations;
using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Linq;
using System.Net;
using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : HospitalBaseController
    {
        public AppointmentController(IHttpContextAccessor httpContextAccessor, IHospitalUnitOfWork hospitalUnitOfWork) : base(httpContextAccessor, hospitalUnitOfWork)
        {
        }

        [HttpGet, Route("AvailableDoctors")]
        public async Task<ServiceActionResult<List<TimeSpan>>> Get_AvailableDoctors([FromQuery] Get_AvailableDoctors_Request request)
        {
            try
            {
                // Get the doctor from the database
                var doctor = await _hospitalUnitOfWork.DoctorRepository.GetOneAsync(c=>c.Id==request.DoctorId);

                if (doctor == null) return new ServiceActionResult<List<TimeSpan>>(new List<TimeSpan>(), 0);


                // Get the appointments for the selected doctor on the selected date
                var appointments = await _hospitalUnitOfWork.Appointment.GetAsync(a => a.DoctorId == doctor.Id && a.StartTime.Date == request.Date.Date);

                // Get the doctor's working hours for the selected day
                var workingHours = doctor.WorkingHours.FirstOrDefault(wh => wh.DayOfWeek == request.Date.DayOfWeek);

                if (workingHours == null)
                {
                    return new ServiceActionResult<List<TimeSpan>>(new List<TimeSpan>(), 0);
                }

                // Get the start time and end time for the doctor's working hours
                var startTime = workingHours.StartTime;
                var endTime = workingHours.EndTime;

                // Get the duration of each appointment in minutes
                var appointmentDuration = request.HoursTakes * 60;

                // Create a list to hold the available time slots
                var availableTimeSlots = new List<TimeSpan>();

                // Loop through the time slots from the start time to the end time, in increments of the appointment duration
                for (var time = startTime; time.AddMinutes(appointmentDuration) <= endTime; time = time.AddMinutes(appointmentDuration))
                {
                    // Check if the current time slot is already taken by an appointment
                    if (appointments.Any(a => a.StartTime.TimeOfDay == time))
                    {
                        continue;
                    }

                    // Add the current time slot to the list of available time slots
                    availableTimeSlots.Add(time);
                }

                return new ServiceActionResult<List<TimeSpan>>(availableTimeSlots);
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<List<TimeSpan>>();
            }
        }






    }
}
