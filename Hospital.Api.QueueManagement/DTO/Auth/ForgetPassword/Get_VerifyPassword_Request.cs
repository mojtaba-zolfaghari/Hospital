namespace Inventory.Api.DTO.Auth.ForgetPassword
{
    /// <summary>
    /// Get_VerifyPassword_Request
    /// </summary>
    public class Get_VerifyPassword_Request
    {
        /// <summary>
        /// Key
        /// </summary>
        public Guid ResetKey { get; set; }
    }
}
