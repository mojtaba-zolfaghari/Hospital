namespace Hospital.Api.QueueManagement.DTO.Auth.Role
{
    /// <summary>
    /// Get_UserRoleList_Response
    /// </summary>
    public class Get_UserRoleList_Response
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// RoleList
        /// </summary>
        public ICollection<RoleList> RoleList { get; set; }
    }
    /// <summary>
    /// RoleList
    /// </summary>
    public class RoleList
    {
        /// <summary>
        /// RoleId
        /// </summary>
        public Guid RoleId { get; set; }
        /// <summary>
        /// RoleName
        /// </summary>
        public string RoleName { get; set; }
    }
}
