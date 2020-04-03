using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Betto.Helpers.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Betto.Helpers
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ApplicationMainConfiguration _appConfiguration;

        public TokenGenerator(IOptions<ApplicationMainConfiguration> appConfiguration)
        {
            _appConfiguration = appConfiguration.Value;
        }

        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appConfiguration.AuthenticationSecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.Now.AddHours(_appConfiguration.AuthenticationTokenValidityTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
