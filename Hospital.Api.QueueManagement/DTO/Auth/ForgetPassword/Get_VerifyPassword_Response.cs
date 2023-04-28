namespace Inventory.Api.DTO.Auth.ForgetPassword
{
    /// <summary>
    /// Get_VerifyPassword_Response
    /// </summary>
    public class Get_VerifyPassword_Response
    {
        /// <summary>
        /// ResetKey
        /// </summary>
        public Guid ResetKey { get; set; }
        /// <summary>
        /// IsValid
        /// </summary>
        public bool IsValid { get; set; }
    }
}
