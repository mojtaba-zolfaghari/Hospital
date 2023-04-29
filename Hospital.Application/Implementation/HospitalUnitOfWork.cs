using Hospital.Application.Implementation.Auth;
using Hospital.Application.Implementation.Doctor;
using Hospital.Application.Implementation.Logger;
using Hospital.Application.Interfaces;
using Hospital.Application.Interfaces.Auth;
using Hospital.Application.Interfaces.Doctor;
using Hospital.Application.Interfaces.Logger;
using Hospital.Infrastructure.Contexts;
using Hospital.Shared.Repository;
using Inventory.Application.Implementation.Auth;
using Inventory.Application.Interfaces.Auth;

namespace Hospital.Application.Implementation
{
    public class HospitalUnitOfWork : UnitOfWork, IHospitalUnitOfWork
    {

        public HospitalUnitOfWork(QueueDbContext hotelContext) : base(hotelContext)
        {
        }
        #region Auth
        private IRoleRoutePermissionRepository _roleRoutePermissionRepository;
        public IRoleRoutePermissionRepository RoleRoutePermissionRepository => _roleRoutePermissionRepository ??= new RoleRoutePermissionRepository(dbContext);

        private IApiUserRepository _apiUserRepository;
        public IApiUserRepository ApiUserRepository => _apiUserRepository ??= new ApiUserRepository(dbContext);

        private IUserRepository _userRepository;
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(dbContext);
        #endregion

        #region Logger
        private ILoggerRepository _loggerRepository;
        public ILoggerRepository LoggerRepository => _loggerRepository ??= new LoggerRepository(dbContext);



        #endregion

        private IRouteRepository _routeRepository;
        public IRouteRepository RouteRepository => _routeRepository ??= new RouteRepository(dbContext);

        #region Role
        private IRoleRouteRepository _roleRouteRepository;
        public IRoleRouteRepository RoleRouteRepository => _roleRouteRepository ??= new RoleRouteRepository(dbContext);

        private IRoleRepository _roleRepository;
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(dbContext);

        private IUserRoleRepository _userRoleRepository;
        public IUserRoleRepository UserRoleRepository => _userRoleRepository ??= new UserRoleRepository(dbContext);



        #endregion
        #region Doctor
        private IDoctorRepository _doctorRepository;
        public IDoctorRepository DoctorRepository => _doctorRepository??=new DoctorRepository(dbContext);

        private IWorkingHourRepository _workingHourRepository;
        public IWorkingHourRepository WorkingHourRepository => _workingHourRepository??= new WorkingHourRepository(dbContext);
        #endregion
    }
}