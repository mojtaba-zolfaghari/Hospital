using Infrastructure.Utitlities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hospital.Shared.Utitlities
{
    /// <summary>
    /// AuthUtilities
    /// </summary>
    public static class AuthUtilities
    {
        public static JwtSecurityToken GetToken(List<Claim> authClaims, string issuer, string audience, int expiresPerMinute)
        {

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetConfigurationManager().GetSection("JWT:Secret").Value));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.UtcNow.AddMinutes(expiresPerMinute),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PasswordLength"></param>
        /// <returns></returns>
        public static string CreateRandomPassword()
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new();
            char[] chars = new char[5];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < 5; i++)
            {
                chars[i] = _allowedChars[(int)(_allowedChars.Length * randNum.NextDouble())];
            }
            return new string(chars);
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
