using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Response;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

/// <summary>
/// Authentication endpoints for JWT-based authentication
/// Implements login and logout with comprehensive logging
/// Database-driven RBAC is validated via PermissionMiddleware
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IAuditService auditService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IAuditService _auditService = auditService;

    /// <summary>
    /// Login endpoint - Issues JWT token upon successful authentication
    /// POST /api/auth/login
    /// </summary>
    /// <param name="request">Login credentials (username, password)</param>
    /// <returns>JWT token with user details</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            if (result is null)
            {
                await _auditService.LogActionAsync("LOGIN_FAILED", "User", $"Failed login attempt for: {request.Username}", "system");
                return Unauthorized(ApiResponse<object>.Failure("Invalid credentials", "401"));
            }

            await _auditService.LogActionAsync("LOGIN_SUCCESS", "User", $"User logged in: {request.Username}", request.Username);
            return Ok(ApiResponse<object>.Success(result, "Login successful"));
        }
        catch (Exception ex)
        {
            await _auditService.LogActionAsync("LOGIN_ERROR", "User", $"Login error: {ex.Message}", "system");
            return BadRequest(ApiResponse<object>.Failure("Login failed", "400"));
        }
    }

    /// <summary>
    /// Logout endpoint - Logs out user and invalidates session
    /// Client must discard stored JWT token
    /// POST /api/auth/logout
    /// </summary>
    /// <param name="username">Username of user logging out</param>
    /// <returns>Logout success confirmation</returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromQuery] string username)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest(ApiResponse<object>.Failure("Username required", "400"));

            var success = await _authService.LogoutAsync(username);
            if (!success)
            {
                await _auditService.LogActionAsync("LOGOUT_FAILED", "User", $"Logout failed: {username}", "system");
                return NotFound(ApiResponse<object>.Failure("User not found", "404"));
            }

            await _auditService.LogActionAsync("LOGOUT_SUCCESS", "User", $"Logged out: {username}", username);
            return Ok(ApiResponse<object>.Success(null, "Logout successful"));
        }
        catch (Exception ex)
        {
            await _auditService.LogActionAsync("LOGOUT_ERROR", "User", $"Logout error: {ex.Message}", "system");
            return BadRequest(ApiResponse<object>.Failure("Logout failed", "400"));
        }
    }
}
