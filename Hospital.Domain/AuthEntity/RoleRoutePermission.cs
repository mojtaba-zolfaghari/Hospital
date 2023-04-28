using Hospital.Shared.Shared;
using System.Data.Entity.Core.Metadata.Edm;

namespace Hospital.Domain.AuthEntity
{
    public class RoleRoutePermission : BaseEntity
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        public OperationAction OperationAction { get; set; }

        public Guid RouteId { get; set; }
        public Route Route { get; set; }
    }
}
