namespace Hospital.Api.QueueManagement.DTO.Auth.RouteAccess
{
    /// <summary>
    /// Edit_RouteAccess_Request
    /// </summary>
    public class Edit_RouteAccess_Request
    {
        /// <summary>
        /// RouteAccessId
        /// </summary>
        public Guid RouteAccessId { get; set; }
        /// <summary>
        /// ControllerName
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// ActionName
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// IpLimited
        /// </summary>
        public bool IpLimited { get; set; }
    }
}
