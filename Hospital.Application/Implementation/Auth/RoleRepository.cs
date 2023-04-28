using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;
using Inventory.Application.Interfaces.Auth;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Implementation.Auth
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
