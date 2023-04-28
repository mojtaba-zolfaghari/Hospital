using ElmahCore;
using Hospital.Api.QueueManagement.DTO.Doctor;
using Hospital.Api.QueueManagement.Utilities;
using Hospital.Application.Interfaces;
using Hospital.Domain.DoctorEntity;
using Hospital.Domain.QueueEntity;
using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Linq;
using System.Net;

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
            try
            {
                var availableDoctors = await _hospitalUnitOfWork.DoctorRepository.GetAvailableDoctors(date);
                var response = availableDoctors.Select(d => new Get_AvailableDoctors_Response
                {
                    Id = d.Id,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    AvailableSlots = d.WorkingHours
                        .Where(w => w.DayOfWeek == date.DayOfWeek)
                        .Select(w => new AvailableSlot
                        {
                            StartTime = date.Date.Add(w.StartTime),
                            EndTime = date.Date.Add(w.EndTime),
                            Capacity = w.Capacity - d.Appointments.Count(a => a.StartTime.Date == date && a.WorkingHour.Id == w.Id),
                            BookedHours = d.Appointments
                                .Where(a => a.StartTime.Date == date && a.WorkingHour.Id == w.Id)
                                .Select(a => new BookedSlot
                                {
                                    StartTime = a.StartTime,
                                    EndTime = a.EndTime,
                                    PatientName = a.Patient.FirstName + " " + a.Patient.LastName,
                                    PatientPhoneNumber = a.Patient.PhoneNumber,
                                    NationalCode = a.Patient.NationalCode
                                }).ToList()
                        }).ToList()
                }).ToList();

                return new ServiceActionResult<List<Get_AvailableDoctors_Response>>(response);
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<List<Get_AvailableDoctors_Response>>();
            }
        }




    }
}
