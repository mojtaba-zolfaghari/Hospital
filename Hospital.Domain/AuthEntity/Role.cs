using Hospital.Shared.Shared;

namespace Hospital.Domain.AuthEntity
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<RoleRoute> UserRoleActions { get; set; }

    }
}
