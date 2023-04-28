namespace Inventory.Api.DTO.Auth.ForgetPassword
{
    /// <summary>
    /// change password
    /// </summary>
    public class Command_ChangePassword_Request
    {
        /// <summary>
        /// ResetKey
        /// </summary>
        public Guid ResetKey { get; set; }
        /// <summary>
        /// NewPassword
        /// </summary>
        public string NewPassword { get; set; }
    }
}
