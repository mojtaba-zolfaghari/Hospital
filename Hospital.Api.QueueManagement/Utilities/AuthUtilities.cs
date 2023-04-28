using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Linq;
using Infrastructure.Utitlities;
using Hospital.Api.QueueManagement.DTO.Auth;
using Hospital.Application.Interfaces;

namespace Hospital.Api.QueueManagement.Utilities
{
    /// <summary>
    /// AuthUtilities
    /// </summary>
    public static class AuthUtilities
    {
        /// <summary>
        /// GetAllControllersAndTheirActions
        /// </summary>
        /// <returns></returns>
        public static List<ControllerAndItsActions> GetAllControllersAndTheirActions()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            IEnumerable<Type> controllers = asm.GetTypes().Where(type => type.Name.EndsWith("Controller"));
            var theList = new List<ControllerAndItsActions>();

            foreach (Type curController in controllers)
            {
                List<string> actions = curController.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)
                    .Where(m => m.CustomAttributes.Any(a => typeof(HttpMethodAttribute).IsAssignableFrom(a.AttributeType)))
                    .Select(x => x.Name)
                    .ToList();

                theList.Add(new ControllerAndItsActions(curController.Name.Replace("Controller", ""), actions));
            }

            return theList;
        }
        internal static JwtSecurityToken GetToken(List<Claim> authClaims)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetConfigurationManager().GetSection("JWT:Secret").Value));

            var token = new JwtSecurityToken(
                issuer: Configuration.GetConfigurationManager().GetSection("JWT:ValidIssuer").Value,
                audience: Configuration.GetConfigurationManager().GetSection("JWT:ValidAudience").Value,
                expires: DateTime.UtcNow.AddHours(int.Parse(Configuration.GetConfigurationManager().GetSection("JWT:ExpirePerHour").Value)),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        /// <summary>
        /// GenerateRefreshToken
        /// </summary>
        /// <returns></returns>
        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        /// <summary>
        /// token passed to this method and we returns the data which is expired and needs to generate new token for irsa
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetConfigurationManager().GetSection("JWT:Secret").Value)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

    }
}
