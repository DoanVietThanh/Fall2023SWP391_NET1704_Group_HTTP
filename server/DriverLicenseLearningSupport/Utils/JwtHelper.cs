using DriverLicenseLearningSupport.Models;
using DriverLicenseLearningSupport.Models.Config;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DriverLicenseLearningSupport.Utils
{
    public class JwtHelper
    {
        private readonly AppSettings _appSettings;

        public JwtHelper(AppSettings appSettings)
        {
           _appSettings = appSettings;
        }

        public string GenerateToken(AccountModel account)
        {
            // secret key
            var secretKey = _appSettings.SecretKey;
            var secetKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            // token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            // token discriptor
            var tokenDiscriptor = new SecurityTokenDescriptor()
            {
                // token details (email, role, tokenId)
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role.Name),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secetKeyBytes),
                    SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GeneratePasswordResetToken(string email) 
        {
            // secret key bytes
            var secretKey = _appSettings.SecretKey;
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
            // token handler
            var tokenHandler = new JwtSecurityTokenHandler();
            // token discriptor
            var tokenDiscription = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("TokenId", Guid.NewGuid().ToString())
                }),
                Expires = DateTime.Now.AddMinutes(10),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKeyBytes),
                    SecurityAlgorithms.HmacSha256)
            };
            // generate token
            var passwordResetToken = tokenHandler.CreateToken(tokenDiscription);
            return tokenHandler.WriteToken(passwordResetToken);
        }
    }
}
