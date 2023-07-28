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
using System.Collections.Generic;
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
        [HttpPost("Available")]
        public async Task<ServiceActionResult<List<Get_AvailableDoctors_Response>>> GetAvailableDoctors(DateTime date)
        {
            try
            {
                var appointementFree = _hospitalUnitOfWork.Appointment.Get(c => !c.IsConfirmed && c.Doctor.IsAvailable, null, null, null, c => c.Doctor);

                var response = appointementFree.Select(d => new Get_AvailableDoctors_Response
                {
                    Id = d.Id,
                    FirstName = d.Doctor.FirstName,
                    LastName = d.Doctor.LastName,
                }).ToList();
                return new ServiceActionResult<List<Get_AvailableDoctors_Response>>(response, "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<List<Get_AvailableDoctors_Response>>();
            }
        }

        [HttpPost("GetAppointment")]
        public async Task<ServiceActionResult<string>> AddDoctorAvailable(Add_Appointment_Request request)
        {
            try
            {
                var selectDate = await _hospitalUnitOfWork.Appointment.GetAsync(
                     c => c.DoctorId == request.DoctorId &&
                     c.Date == request.Date &&
                     !c.IsConfirmed
                     , null, null, null);
                if (selectDate.Count() < 1) return new ServiceActionResult<string>("no appointment available!", HttpStatusCode.Conflict);

                Patient patient = null;
                var existPatint = await _hospitalUnitOfWork.PatientRepository.GetOneAsync(c => c.NationalCode == request.NationalCode);

                if (existPatint != null)
                {
                    patient = existPatint;
                }
                else
                {
                    patient = new Patient
                    {
                        Birthdate = request.Birthdate,
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        NationalCode = request.NationalCode,
                        Gender = request.Gender,
                        PhoneNumber = request.PhoneNumber,
                    };
                }

                if (_hospitalUnitOfWork.Appointment.Get(
                     c => c.DoctorId == request.DoctorId &&
                     c.Date == request.Date &&
                     c.Patient.NationalCode == request.NationalCode,null,null,null,c=>c.Patient).Count() > 0)
                {
                     return new ServiceActionResult<string>("You can reserve every 24 hours for each doctor!", HttpStatusCode.Conflict);
                }

                var newQ = selectDate.FirstOrDefault();
                newQ.Patient = patient;
                newQ.PatientId = patient.Id;
                newQ.DoctorId = request.DoctorId;
                newQ.Status = Domain.Shared.SharedEnums.AppointmentStatus.Requested;
                 _hospitalUnitOfWork.Appointment.Update(newQ);
                if (existPatint == null)_hospitalUnitOfWork.PatientRepository.Create(patient);
                await _hospitalUnitOfWork.SaveAsync();
                return new ServiceActionResult<string>(newQ.QueuingNumber.ToString(), "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }
        [HttpPost("AddDoctorAvailable")]
        public async Task<ServiceActionResult<string>> AddDoctorAvailable(Add_DoctorAvailable_Request request)
        {
            try
            {
                for (int i = 0; i < request.Count; i++)
                {
                    _hospitalUnitOfWork.Appointment.Create(new Appointment
                    {
                        DoctorId = request.DoctorId,
                        Date = request.Date,
                        IsConfirmed = false,
                        Status = Domain.Shared.SharedEnums.AppointmentStatus.NotBook,
                        PatientId = null,
                        QueuingNumber = 0
                    });
                }
                await _hospitalUnitOfWork.SaveAsync();


                return new ServiceActionResult<string>(null, "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }

        [HttpPut("ConfirmAppointment")]
        public async Task<ServiceActionResult<string>> ConfirmAppointment(Command_ConfirmAppointment_Request request)
        {
            try
            {
                var selectedAppointment = await _hospitalUnitOfWork.Appointment.GetOneAsync(c => c.Id == request.AppointmentId);
                int lastQ = 0;
                var lastQuList = await _hospitalUnitOfWork.Appointment.GetAsync(
                     c => c.DoctorId == selectedAppointment.DoctorId &&
                     c.Date == selectedAppointment.Date &&
                     c.IsConfirmed
                     , null, null, null);

                if (lastQuList.Count() > 0) lastQ = lastQuList.Max(c => c.QueuingNumber);

                var NewQNumber = lastQ + 1;
                selectedAppointment.IsConfirmed = true;
                selectedAppointment.Status = request.Status;
                selectedAppointment.QueuingNumber = NewQNumber;
                _hospitalUnitOfWork.Appointment.Update(selectedAppointment);
                await _hospitalUnitOfWork.SaveAsync();


                return new ServiceActionResult<string>(NewQNumber.ToString(), "DONE");
            }
            catch (Exception ex)
            {
                await _httpContextAccessor.HttpContext.RaiseError(ex);
                return new ServiceActionResult<string>();
            }
        }



    }
}
