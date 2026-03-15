using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Infrastructure.Authentication;

public class JwtTokenService(IConfiguration config) : ITokenService
{
    private readonly IConfiguration _config = config;

    public string GenerateToken(string username, string role, Dictionary<string, string>? additionalClaims = null)
    {
        var secretKey = _config["JwtSettings:SecretKey"] 
            ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (additionalClaims != null)
        {
            foreach (var claim in additionalClaims)
            {
                claims.Add(new Claim(claim.Key, claim.Value));
            }
        }

        var expiryMinutes = double.Parse(_config["JwtSettings:ExpiryMinutes"] ?? "60");
        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (bool IsValid, string? Username, string? Role) ValidateToken(string token)
    {
        try
        {
            var secretKey = _config["JwtSettings:SecretKey"] 
                ?? throw new InvalidOperationException("JwtSettings:SecretKey is not configured");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = _config["JwtSettings:Issuer"] ?? throw new InvalidOperationException("JwtSettings:Issuer is not configured"),
                ValidateAudience = true,
                ValidAudience = _config["JwtSettings:Audience"] ?? throw new InvalidOperationException("JwtSettings:Audience is not configured"),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            var username = principal.FindFirst(ClaimTypes.Name)?.Value;
            var role = principal.FindFirst(ClaimTypes.Role)?.Value;

            return (true, username, role);
        }
        catch
        {
            return (false, null, null);
        }
    }

    public string RefreshToken(string username, string role)
    {
        return GenerateToken(username, role);
    }
}

