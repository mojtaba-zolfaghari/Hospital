namespace Inventory.Api.DTO.Auth.Role
{
    /// <summary>
    /// 
    /// </summary>
    public class Add_UserRole_Request
    {
        /// <summary>
        /// UserId
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// RoleId
        /// </summary>
        public Guid RoleId { get; set; }
    }
}
