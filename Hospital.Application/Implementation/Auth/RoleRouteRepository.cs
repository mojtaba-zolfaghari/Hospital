using Hospital.Application.Interfaces.Auth;
using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Implementation.Auth
{
    public class RoleRouteRepository : Repository<RoleRoute>, IRoleRouteRepository
    {
        public RoleRouteRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
