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

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddDoctor"), HospitalAuthorization]
        public async Task<ServiceActionResult<string>> AddUser(Add_Doctor_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);

                if (_hospitalUnitOfWork.DoctorRepository.GetExists(c => c.FirstName == request.FirstName))
                    return new ServiceActionResult<string>("this username already exist!", HttpStatusCode.Conflict);

                if (_hospitalUnitOfWork.UserRepository.GetOne(c => c.UserDetail.Email == request.Email, e => e.UserDetail) != null)
                    return new ServiceActionResult<string>("this email already exist!", HttpStatusCode.Conflict);

                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);
                if (hasAccessToCreateHotel)
                {
                    User user = new()
                    {
                        Username = request.Username,
                        //IPList = request.IPList,
                        //LanguageId = request.LanguageId,
                        Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                        Name = request.Name + " " + request.LastName,
                        Active = true,
                        EnForceChangePassword = true,
                        UserDetail = new UserDetail
                        {
                            FirstName = request.Name,
                            LastName = request.LastName,
                            Email = request.Email
                        }
                    };

                    _hospitalUnitOfWork.UserRepository.Create(user);

                    await _hospitalUnitOfWork.SaveAsync();
                    return new ServiceActionResult<string>(null, "DONE");
                }
                else
                {
                    return new ServiceActionResult<string>("Access denied", HttpStatusCode.Forbidden);
                }
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }



    }
}
