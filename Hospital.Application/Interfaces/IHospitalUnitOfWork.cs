using Hospital.Application.Implementation.Auth;
using Hospital.Application.Interfaces.Auth;
using Hospital.Application.Interfaces.Doctor;
using Hospital.Application.Interfaces.Logger;
using Hospital.Shared.Repository;
using Inventory.Application.Interfaces.Auth;

namespace Hospital.Application.Interfaces
{
    public interface IHospitalUnitOfWork : IUnitOfWork
    {
        #region Auth
        IApiUserRepository ApiUserRepository { get; }
        IRoleRoutePermissionRepository RoleRoutePermissionRepository { get; }
        #endregion

        #region User
        IUserRepository UserRepository { get; }
        #endregion

        #region Logger
        ILoggerRepository LoggerRepository { get; }

        #endregion

        #region Role
        IRoleRepository RoleRepository { get; }
        IRouteRepository RouteRepository { get; }
        IRoleRouteRepository RoleRouteRepository { get; }
        IUserRoleRepository UserRoleRepository { get; }
        #endregion

        #region Doctor
        IDoctorRepository DoctorRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        IPatientRepository PatientRepository { get; }
        #endregion
    }
}
