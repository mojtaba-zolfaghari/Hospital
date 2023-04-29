using ElmahCore;
using Hospital.Api.QueueManagement.DTO.Doctor;
using Hospital.Api.QueueManagement.Utilities;
using Hospital.Application.Interfaces;
using Hospital.Domain.DoctorEntity;
using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static Hospital.Api.QueueManagement.DTO.Doctor.Add_WorkingHour_Request;

namespace Hospital.Api.QueueManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : HospitalBaseController
    {
        public DoctorController(IHttpContextAccessor httpContextAccessor, IHospitalUnitOfWork hospitalUnitOfWork) : base(httpContextAccessor, hospitalUnitOfWork)
        {
        }
        /// <summary>
        /// Add doctor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddDoctor")/*, HospitalAuthorization*/]
        public async Task<ServiceActionResult<string>> AddDoctor(Add_Doctor_Request request)
        {
            try
            {
                if (request == null) return new ServiceActionResult<string>("model properties is null", HttpStatusCode.BadRequest);

                if (_hospitalUnitOfWork.DoctorRepository.GetExists(c => c.FirstName == request.FirstName && c.LastName == request.LastName))
                    return new ServiceActionResult<string>("this doctor already exist!", HttpStatusCode.Conflict);


                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);
                if (hasAccessToCreateHotel)
                {
                    Doctor doctor = new()
                    {
                        LastName = request.LastName,
                        FirstName = request.FirstName,
                        CapacityPerDay = request.CapacityPerDay,
                    };

                    _hospitalUnitOfWork.DoctorRepository.Create(doctor);

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

        /// <summary>
        /// Add working hours for a doctor
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, Route("AddWorkingHoursForDoctor")/*, HospitalAuthorization*/]
        public async Task<ServiceActionResult<string>> AddWorkingHoursForDoctor(Add_WorkingHours_Request request)
        {
            try
            {
                if (request == null || request.DoctorId == Guid.Empty || request.WorkingHours == null || request.WorkingHours.Count == 0)
                {
                    return new ServiceActionResult<string>("Invalid request data", HttpStatusCode.BadRequest);
                }

                var doctor = await _hospitalUnitOfWork.DoctorRepository.GetOneAsync(c=>c.Id== request.DoctorId,c=>c.WorkingHours);
                if (doctor == null) return new ServiceActionResult<string>("Doctor not found", HttpStatusCode.NotFound);

                var currentUserId = GeneralUtilities.GetCurrentUserId(_httpContextAccessor);
                var hasAccessToCreateHotel = _hospitalUnitOfWork.UserRepository.hasAccessToCurrentOPeration(currentUserId);

                if (hasAccessToCreateHotel)
                {
                    // Remove existing working hours for the doctor
                    doctor.WorkingHours.ToList().ForEach(w => _hospitalUnitOfWork.WorkingHourRepository.Delete(w));

                    // Add new working hours
                    request.WorkingHours.ForEach(workingHour =>
                    {
                        _hospitalUnitOfWork.WorkingHourRepository.Create(new WorkingHour
                        {
                            Capacity = workingHour.Capacity,
                            DoctorId = request.DoctorId,
                            StartTime = workingHour.StartTime,
                            EndTime = workingHour.EndTime,
                            DayOfWeek = workingHour.DayOfWeek,
                        });
                    });

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
