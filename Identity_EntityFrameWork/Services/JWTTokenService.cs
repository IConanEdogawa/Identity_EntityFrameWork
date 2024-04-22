using Identity_EntityFrameWork.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Identity_EntityFrameWork.Services
{
    public class JWTTokenService
    {
        private readonly IConfiguration _configuration;

        public JWTTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JWTToken GenerateToken(AppUser user)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]!);
            var issuer = _configuration["JwtSettings:Issuer"];
            var expirationHours = _configuration.GetValue<int>("JwtSettings:ExpirationHours");

            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                // Add user roles as claims
                new Claim(ClaimTypes.Role, string.Join(",", user.Roles.Select(r => r.Name)))
            };
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(expirationHours),
                Issuer = issuer,
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
