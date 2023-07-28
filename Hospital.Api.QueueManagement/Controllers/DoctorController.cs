using ElmahCore;
using Hospital.Api.QueueManagement.DTO.Doctor;
using Hospital.Api.QueueManagement.Utilities;
using Hospital.Application.Interfaces;
using Hospital.Domain.DoctorEntity;
using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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

        [HttpPost, Route("Add")/*, HospitalAuthorization*/]
        public async Task<ServiceActionResult<string>> Add(Add_Doctor_Request request)
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
                        IsAvailable = false
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

        [HttpGet, Route("Get")/*, HospitalAuthorization*/]
        public async Task<ServiceActionResult<Get_Doctor_Response>> Get([FromHeader] Get_Doctor_Request request)
        {
            try
            {
                var doctor = await _hospitalUnitOfWork.DoctorRepository.GetByIdAsync(request.DoctorId);
                if (doctor == null) return new ServiceActionResult<Get_Doctor_Response>("Doctor not found", HttpStatusCode.NotFound);

                var doctorDTO = new Get_Doctor_Response
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    CapacityPerDay = doctor.CapacityPerDay,
                };
                return new ServiceActionResult<Get_Doctor_Response>(doctorDTO, "DONE");


            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<Get_Doctor_Response>();
            }

        }

        [HttpGet, Route("List")/*, HospitalAuthorization*/]
        public async Task<ServiceActionResult<ListResult<Get_Doctor_Response>>> List([FromQuery] Get_Doctors_Request request)
        {
            try
            {
                Expression buildingPredicate = request.Filters.ToPredicate<Doctor>(typeof(Doctor));

                var doctors = await _hospitalUnitOfWork.DoctorRepository.GetAsync((Expression<Func<Doctor, bool>>)buildingPredicate, c => c.OrderBy(request.OrderBy + " " + request.SortType), request.Skip, request.Take);

                var totalCount = await _hospitalUnitOfWork.DoctorRepository.GetCountAsync();
                var doctorResponses = doctors.Select(doctor => new Get_Doctor_Response
                {
                    Id = doctor.Id,
                    FirstName = doctor.FirstName,
                    LastName = doctor.LastName,
                    CapacityPerDay = doctor.CapacityPerDay,
                }).ToList();

                var listResult = new ListResult<Get_Doctor_Response>(doctorResponses, totalCount);

                return new ServiceActionResult<ListResult<Get_Doctor_Response>>(listResult);
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<ListResult<Get_Doctor_Response>>();
            }
        }
    }





}

