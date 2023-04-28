

using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Hospital.Shared.Utitlities.Attributes;

namespace Hospital.Domain.AuthEntity
{
    public class UserRole : BaseEntity
    {
        [OperatorComparer(PublicEnumes.OperatorComparer.Equals)]
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
        [OperatorComparer(PublicEnumes.OperatorComparer.Equals)]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
