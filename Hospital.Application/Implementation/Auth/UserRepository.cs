using Hospital.Application.Interfaces.Auth;
using Hospital.Domain.AuthEntity;
using Hospital.Infrastructure.Contexts;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Implementation.Auth
{

    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext dbContext) : base(dbContext)
        { }

        public Guid hasAccessToCurrentAction(string ControllerName, Guid RoleId, string ActionName)
        {
            var hotelContext = Context as QueueDbContext;

            var exist = hotelContext.UserRoles
                .Where(c => c.RoleId == RoleId)
                .Include(c => c.Role.UserRoleActions
                .Where(c => c.Route.ControllerName == ControllerName && c.Route.ActionName == ActionName && c.RoleId == RoleId))
                //.Where(c => c.RoleId == RoleId)
                .ToList();

            if (exist.FirstOrDefault().Role.UserRoleActions.Count > 0) return exist.FirstOrDefault().Role.UserRoleActions.FirstOrDefault().RouteId;

            return Guid.Empty;
        }

        public bool hasAccessToCurrentOPeration(Guid UserId)
        {
            var hotelContext = Context as QueueDbContext;

            var exist = (from usrrole in hotelContext.UserRoles
                         where usrrole.UserId == UserId
                         select new
                         {
                             usrrole.UserId,
                             Role = usrrole.Role.Name,
                         }).ToList();

            foreach (var item in exist)
            {
                if (item.Role == "Admin") return true;
                else if (item.Role == "Vendor") return true;
            }
            return false;
        }
        public bool hasAccessToAddHotel(Guid UserId)
        {
            var hotelContext = Context as QueueDbContext;

            //var exist = (from usrrole in hotelContext.UserRoles
            //             join htl in hotelContext.VendorHotels
            //             on usrrole.UserId equals htl.Vendor.UserId
            //             where usrrole.UserId == UserId
            //             select new
            //             {
            //                 StayType = htl.Hotel.HotelType,
            //                 usrrole.UserId,
            //                 Role = usrrole.Role.Name,
            //             }).ToList();
            var userInRole = hotelContext.UserRoles.Where(c => c.UserId == UserId)
                .Join(hotelContext.Roles,
                      ur => ur.RoleId, rl => rl.Id,
                      (ur, rl) =>
                      new
                      {
                          roleName = rl.Name,

                      });
            foreach (var item in userInRole)
            {
                if (item.roleName == "Admin" || item.roleName == "Vendor") return true;
            }
            //foreach (var item in exist)
            //{
            //    if (item.StayType == Domain.Shared.Stay_Type.Hotel && item.Role == "Admin") return true;
            //    else if (item.StayType == Domain.Shared.Stay_Type.Hotel && item.Role == "Vendor") return true;
            //}
            return false;
        }

        List<UserRole> IUserRepository.GetUserRoles(Guid UserId)
        {
            return Context.Set<UserRole>()
                .Where(c => c.UserId == UserId)
                .Include(c => c.Role).ToList();
        }
    }
}
