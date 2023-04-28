using Hospital.Shared.Shared;

namespace Hospital.Domain.AuthEntity
{
    public class RoleRoute : BaseEntity
    {
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }

        public Guid RouteId { get; set; }
        public virtual Route Route { get; set; }
    }
}
