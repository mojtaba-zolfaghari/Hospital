namespace Hospital.Api.QueueManagement.DTO.Auth.RouteAccess
{
    /// <summary>
    /// Add_RouteAcces_Request
    /// </summary>
    public class Add_RouteAcces_Request
    {
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
