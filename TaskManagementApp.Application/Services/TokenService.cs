using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApp.Application.Helpers;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.Application.Models;
using TaskManagementApp.Domain.Entities;

namespace TaskManagementApp.Application.Services
{
    public class TokenService : ITokenService
    {
        private const string JwtKeyConfigName = "JwtSettings:Key"; 
        private const string JwtIssuerConfigName = "JwtSettings:Issuer";
        private const string JwtAudienceConfiName = "JwtSettings:Audience";

        public TokenService() {}

        public AuthResponse GenerateToken(User user)
        {
            var jwtKey = ConfigurationHelper.config.GetSection(JwtKeyConfigName).Value;  //_configuration[JwtKeyConfigName];
            var jwtIssuer = ConfigurationHelper.config.GetSection(JwtIssuerConfigName).Value;  //_configuration[JwtIssuerConfigName];
            var jwtAudience = ConfigurationHelper.config.GetSection(JwtAudienceConfiName).Value;  //_configuration[JwtAudienceConfiName];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: creds);

            var refreshToken = Guid.NewGuid().ToString();

            // Save the refresh token to the database or other storage with a longer expiration time
            // e.g., 7 days

            return new AuthResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken
            };
        }

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtKey = ConfigurationHelper.config.GetSection(JwtKeyConfigName).Value;  //_configuration[JwtKeyConfigName];

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = false // here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
