using Hospital.Domain.AuthEntity;
using Hospital.Shared.Repository;

namespace Hospital.Application.Interfaces.Auth
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {
    }
}
