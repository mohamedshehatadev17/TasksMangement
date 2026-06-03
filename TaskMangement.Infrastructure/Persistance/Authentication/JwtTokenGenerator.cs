using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskMangement.Application.Abstractions.Authentication;
using TaskMangement.Domain.Models;

namespace TaskMangement.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        public JwtTokenGenerator(IConfiguration configuration, UserManager<User> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            var keyString = _configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is missing");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is missing");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is missing");

            var duration = Convert.ToDouble(
                _configuration["Jwt:DurationInMinutes"] ?? "60");

            // Get User Roles
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),

        new(JwtRegisteredClaimNames.Email,
            user.Email ?? ""),

        new(ClaimTypes.Name,
            user.UserName ?? ""),

        new(JwtRegisteredClaimNames.Jti,
            Guid.NewGuid().ToString()),

        new(JwtRegisteredClaimNames.Iat,
            DateTimeOffset.UtcNow
                .ToUnixTimeSeconds()
                .ToString(),
            ClaimValueTypes.Integer64)
    };

            // Add Roles To Claims
            claims.AddRange(
                roles.Select(role =>
                    new Claim(ClaimTypes.Role, role)));

            var keyBytes = Convert.FromBase64String(keyString);

            var key = new SymmetricSecurityKey(keyBytes);

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(duration),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

    }

}