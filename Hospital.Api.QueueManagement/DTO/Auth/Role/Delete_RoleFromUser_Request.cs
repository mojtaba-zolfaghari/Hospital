namespace Inventory.Api.DTO.Auth.Role
{
    /// <summary>
    /// this model is for delete user from specific role
    /// </summary>
    public class Delete_RoleFromUser_Request
    {
        /// <summary>
        /// the user role (required)
        /// </summary>
        public Guid UserRoleId { get; set; }
    }
}
