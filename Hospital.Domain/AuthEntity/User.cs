using Hospital.Shared.Shared;

namespace Hospital.Domain.AuthEntity
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }
        public bool EnForceChangePassword { get; set; }
        public Guid? ChangePasswordTempId { get; set; }
        public DateTime ChangePasswordTempExpireDate { get; set; }
        //public string IPList { get; set; }

        public string RefreshToken { get; set; }
        public string Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        //public Guid LanguageId { get; set; }
        //public virtual Language Language { get; set; }

        public virtual UserDetail UserDetail { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
