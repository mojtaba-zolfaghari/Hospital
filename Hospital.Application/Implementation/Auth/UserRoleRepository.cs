using Hospital.Application.Interfaces.Auth;
using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Implementation.Auth
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext dbContext) : base(dbContext)
        {

        }
    }
}
