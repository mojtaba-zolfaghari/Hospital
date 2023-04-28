using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;

namespace Hospital.Application.Interfaces.Auth
{
    public interface IUserRepository : IRepository<User>
    {
        List<UserRole> GetUserRoles(Guid UserId);
        Guid hasAccessToCurrentAction(string ControllerName, Guid RoleId, string ActionName);
        bool hasAccessToAddHotel(Guid UserId);
        bool hasAccessToCurrentOPeration(Guid UserId);
    }
}
