using Hospital.Shared.Shared;

namespace Hospital.Domain.AuthEntity
{
    public class UserDetail : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

    }
}
