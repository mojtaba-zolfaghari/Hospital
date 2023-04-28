using Hospital.Application.Interfaces.Auth;
using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Application.Implementation.Auth
{
    public class ApiUserRepository : Repository<ApiUser>, IApiUserRepository
    {
        public ApiUserRepository(DbContext context) : base(context)
        {
        }
    }
}
