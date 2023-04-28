using Hospital.Shared.Shared;
using Hospital.Shared.Utitlities;
using Hospital.Shared.Utitlities.Attributes;

namespace Hospital.Domain.AuthEntity
{
    public class ApiUser : BaseEntity
    {
        [OperatorComparer(PublicEnumes.OperatorComparer.Contains)]
        public string Name { get; set; }
        public string Token { get; set; }
        [OperatorComparer(PublicEnumes.OperatorComparer.GreaterThanOrEqual)]
        public DateTime TokenStartDate { get; set; }
        [OperatorComparer(PublicEnumes.OperatorComparer.LessThanOrEqual)]
        public DateTime TokenExpireDate { get; set; }
        public string RefreshToken { get; set; }
        [OperatorComparer(PublicEnumes.OperatorComparer.GreaterThanOrEqual)]
        public long ApiRequestCount { get; set; }
    }
}
