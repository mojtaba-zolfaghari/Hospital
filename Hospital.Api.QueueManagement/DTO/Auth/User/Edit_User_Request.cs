namespace Hospital.Api.QueueManagement.DTO.Auth.User
{
    /// <summary>
    /// Edit User
    /// </summary>
    public class Edit_User_Request
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// NationalCode
        /// </summary>
        public string NationalCode { get; set; }

        /// <summary>
        /// IPList
        /// </summary>
        public string IPList { get; set; }
        /// <summary>
        /// LanguageId
        /// </summary>
        public Guid LanguageId { get; set; }
    }
}
