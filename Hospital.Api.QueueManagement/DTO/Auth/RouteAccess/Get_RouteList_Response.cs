namespace Hospital.Api.QueueManagement.DTO.Auth.RouteAccess
{
    /// <summary>
    /// Get_RouteList_Response
    /// </summary>
    public class Get_RouteList_Response
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ActionName
        /// </summary>
        public string ActionName { get; set; }
        /// <summary>
        /// ControllerName
        /// </summary>
        public string ControllerName { get; set; }
        /// <summary>
        /// IpLimited
        /// </summary>
        public bool IpLimited { get; set; }
    }
}
