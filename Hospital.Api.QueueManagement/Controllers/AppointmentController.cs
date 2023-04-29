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
        [HttpGet("available")]
        public async Task<ActionResult<ServiceActionResult<List<Get_AvailableDoctors_Response>>>> GetAvailableDoctors(DateTime date)
        {
            var doctors = await _hospitalUnitOfWork.DoctorRepository.GetAvailableDoctors(date);
            var availableDoctors = new List<Get_AvailableDoctors_Response>();

            var dayOfWeekMap = new Dictionary<DayOfWeek, DayOfWeekEnum>
                               {
                                   { DayOfWeek.Monday, DayOfWeekEnum.Monday },
                                   { DayOfWeek.Tuesday, DayOfWeekEnum.Tuesday },
                                   { DayOfWeek.Wednesday, DayOfWeekEnum.Wednesday },
                                   { DayOfWeek.Thursday, DayOfWeekEnum.Thursday },
                                   { DayOfWeek.Friday, DayOfWeekEnum.Friday },
                                   { DayOfWeek.Saturday, DayOfWeekEnum.Saturday },
                                   { DayOfWeek.Sunday, DayOfWeekEnum.Sunday }
                               };

            var dayOfWeek = dayOfWeekMap[date.DayOfWeek];

            foreach (var doctor in doctors)
            {
                var availableSlots = doctor.WorkingHours
                    .Where(w => w.DayOfWeek == dayOfWeek)
                    .Select(w => new AvailableSlot
                    {
                        StartTime = date.Date.Add(w.StartTime),
                        EndTime = date.Date.Add(w.EndTime),
                        Capacity = w.Capacity - doctor.Appointments.Count(a => a.StartTime.Date == date && a.DoctorId == doctor.Id && a.Status == AppointmentStatus.Done)
                    })
                    .ToList();

                var bookedSlots = doctor.Appointments
                    .Where(a => a.StartTime.Date == date && a.Status == AppointmentStatus.Done)
                    .Select(a => new BookedSlot
                    {
                        StartTime = a.StartTime,
                        PatientName = $"{a.Patient.FirstName}+ {a.Patient.LastName}",
                        PatientPhoneNumber = a.Patient.PhoneNumber,
                        NationalCode = a.Patient.NationalCode
                    })
                    .ToList();

                var doctorResponse = new Get_AvailableDoctors_Response
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    AvailableSlots = availableSlots,
                    BookedSlots = bookedSlots
                };

                availableDoctors.Add(doctorResponse);
            }

            return new ServiceActionResult<List<Get_AvailableDoctors_Response>>(availableDoctors);
        }

        

        

    }
}
