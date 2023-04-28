namespace Hospital.Api.QueueManagement.DTO.Auth.User
{
    /// <summary>
    /// Add_User_Request
    /// </summary>
    public class Add_User_Request
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// LastName
        /// </summary>
        public string LastName { get; set; }
        //public string NationalCode { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        //public string IPList { get; set; }

        //public Guid LanguageId { get; set; }

    }
}
