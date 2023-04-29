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
        [HttpPost, Route("AddDoctor")]
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
                    Doctor doctor = new Doctor
                    {
                        LastName = request.LastName,
                        FirstName = request.FirstName,
                        CapacityPerDay = request.CapacityPerDay,
                        WorkingHours = request.WorkingHours?.Select(w => new WorkingHour
                        {
                            DayOfWeek = w.DayOfWeek,
                            StartTime = w.StartTime,
                            EndTime = w.EndTime,
                            Capacity = w.Capacity
                        }).ToList()
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



    }
}
