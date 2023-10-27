using ExpensesReport.Identity.Core.Constants;
using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Infrastructure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExpensesReport.Identity.Application.Services
{
    public class AuthServices
    {
        public static string GenerateToken(UserIdentity userIdentity, string role, IConfiguration config)
        {
            string userPermissions = UserIdentityPermissions.GetPermissions(role).Aggregate((a, b) => $"{a},{b}");

            IEnumerable<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userIdentity.Id.ToString()),
                new Claim(ClaimTypes.Email, userIdentity.Email!),
                new Claim(ClaimTypes.Role, role),
                new Claim("permissions", userPermissions!)
            };

            var jwtKey = config.GetSection("Jwt:Key").Value;
            var issuer = config.GetSection("Jwt:Issuer").Value!;
            var audience = config.GetSection("Jwt:Audience").Value;
            var jwtExpireMinutes = config.GetSection("Jwt:ExpireMinutes").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = issuer,
                Audience = audience,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtExpireMinutes!)),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public static List<Claim> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            return jsonToken!.Claims.ToList();
        }

        public static string GenerateRandomToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var token = Convert.ToBase64String(randomNumber);

            return HashServices.Encrypt(token);
        }
    }
}
