using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;
using Inventory.Application.Interfaces.Auth;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Implementation.Auth
{
    public class RoleRoutePermissionRepository : Repository<RoleRoutePermission>, IRoleRoutePermissionRepository
    {
        public RoleRoutePermissionRepository(DbContext context) : base(context)
        {
        }
    }
}
