namespace EmployeeManagement.Infrastructure.Authentication;

public interface ITokenService
{
    string GenerateToken(string username, string role, Dictionary<string, string>? additionalClaims = null);
    (bool IsValid, string? Username, string? Role) ValidateToken(string token);
    string RefreshToken(string username, string role);
}

