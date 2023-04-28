namespace Hospital.Api.QueueManagement.DTO.Auth
{
    /// <summary>
    /// AssignRoleToRoute_Request
    /// </summary>
    public class AssignRoleToRoute_Request
    {
        /// <summary>
        /// RoleId
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// RouteId
        /// </summary>
        public Guid RouteId { get; set; }
    }
}
