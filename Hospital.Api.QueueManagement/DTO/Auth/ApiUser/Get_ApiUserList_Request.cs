namespace Inventory.Api.DTO.Auth.ApiUser
{
    /// <summary>
    /// Get_ApiUserList_Request
    /// </summary>
    public class Get_ApiUserList_Request
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// token generated for user
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// TokenStartDate
        /// </summary>
        public DateTime TokenStartDate { get; set; }
        /// <summary>
        /// TokenExpireDate
        /// </summary>
        public DateTime TokenExpireDate { get; set; }
        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// ApiRequestCount
        /// </summary>
        public long ApiRequestCount { get; set; }
    }
}
