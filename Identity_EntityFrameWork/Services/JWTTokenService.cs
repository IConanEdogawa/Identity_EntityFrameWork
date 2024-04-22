using Identity_EntityFrameWork.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity_EntityFrameWork.Services
{
    public class JWTTokenService
    {

        private readonly string _key;
        private readonly string _issuer;

        public JWTTokenService(string key, string issuer)
        {
            _key = key;
            _issuer = issuer;
        }

        public JWTToken GenerateToken(string userId, string userName)
        {

            // Agar siz bundan foydalanishni hohlasangiz avval constructorga key va issuerni berishiz kere.

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new JWTToken
            {
                Token = tokenString,
                Expiration = tokenDescriptor.Expires ?? DateTime.UtcNow
            };

        }



    }
}
