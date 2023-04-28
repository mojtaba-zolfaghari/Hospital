using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Api.DTO.Auth.Login
{
    /// <summary>
    /// LoginViewModel_Response
    /// </summary>
    public class LoginViewModel_Response
    {
        /// <summary>
        /// LoginViewModel_Response
        /// </summary>
        public LoginViewModel_Response()
        {
            Roles = new List<LoginRoles_ViewModel>();
        }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// email of the user
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Id of the user
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// RefreshToken
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// Expiration
        /// </summary>

        [Column(TypeName = "smalldatetime")]
        public DateTime Expiration { get; set; }
        /// <summary>
        /// Roles
        /// </summary>
        public List<LoginRoles_ViewModel> Roles { get; set; }

    }
}
