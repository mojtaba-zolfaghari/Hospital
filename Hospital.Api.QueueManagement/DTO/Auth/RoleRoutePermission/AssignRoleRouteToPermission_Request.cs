using static Hospital.Domain.Shared.SharedEnums;

namespace Hospital.Api.QueueManagement.DTO.Auth.RoleRoutePermission
{
    /// <summary>
    /// AssignRoleRouteToPermission_Request
    /// </summary>
    public class AssignRoleRouteToPermission_Request
    {
        /// <summary>
        /// RoleId
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// RouteId
        /// </summary>
        public Guid RouteId { get; set; }
        /// <summary>
        /// OperationAction
        /// </summary>
        public ICollection<OperationAction> OperationsAction { get; set; }
    }
}
