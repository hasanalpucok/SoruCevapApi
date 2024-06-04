using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using SoruCevap.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SoruCevap.Helper.Jwt
{
    public class JwtMethods
    {
        private static readonly string Secret = "c33a8499f41527e05aa6d7b61925fc87";
        public static string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        // Verilen bir JWT'nin geçerliliğini ve belirtilen kullanıcıya ait olup olmadığını kontrol eder
        public static bool IsTokenValid(string token, string userId, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Secret);

            // Token doğrulama parametreleri
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero // Saat kayması kabul etme
            };

            try
            {
                SecurityToken validatedToken;
                // Token doğrulama ve iddiaları çıkarma
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                // Token'ın gerekli iddiaları (userId ve username) içerip içermediğini kontrol eder
                if (principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) &&
                    principal.HasClaim(c => c.Type == ClaimTypes.Name))
                {
                    var tokenUserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var tokenUsername = principal.FindFirst(ClaimTypes.Name)?.Value;

                    // Token'ın userId ve username'inin sağlanan değerlerle eşleşip eşleşmediğini kontrol eder
                    if (tokenUserId == userId && tokenUsername == username)
                    {
                        return true; // Token geçerli ve belirtilen kullanıcıya ait
                    }
                }
            }
            catch (Exception)
            {
                // Token doğrulama başarısız oldu
                return false;
            }

            // Token ya geçersizdir ya da belirtilen kullanıcıya ait değildir
            return false;
        }
    }
}
