using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;

namespace Inventory.Application.Interfaces.Auth
{
    public interface IRoleRoutePermissionRepository : IRepository<RoleRoutePermission>
    {
    }
}
