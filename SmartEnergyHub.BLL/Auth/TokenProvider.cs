using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartEnergyHub.BLL.Auth.Interfaces;
using SmartEnergyHub.DAL.Entities.APIUser;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartEnergyHub.BLL.Auth
{
    public class TokenProvider : ITokenProvider
    {
        public string CreateToken(IConfiguration configuration, User user)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
