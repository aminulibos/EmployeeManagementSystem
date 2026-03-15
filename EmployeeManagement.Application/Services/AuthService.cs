﻿using EmployeeManagement.Application.DTOs.Auth;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Infrastructure.Authentication;
using EmployeeManagement.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordService passwordService, ILogger<AuthService> logger) : IAuthService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IPasswordService _passwordService = passwordService;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Username == request.Username).ConfigureAwait(false);

            if (user is null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Login failed for user: {request.Username}");
                return null;
            }

            var role = await _unitOfWork.Roles.GetByIdAsync(user.RoleId).ConfigureAwait(false);
            if (role is null)
                return null;

            var token = _tokenService.GenerateToken(user.Username, role.Name);

            var response = new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = role.Name,
                ExpiresAt = DateTime.UtcNow.AddMinutes(60)
            };

            _logger.LogInformation($"User logged in successfully: {user.Username}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login");
            throw;
        }
    }

    public async Task<bool> LogoutAsync(string username)
    {
        try
        {
            var userExists = await _unitOfWork.Users.AnyAsync(u => u.Username == username).ConfigureAwait(false);
            if (userExists)
                _logger.LogInformation($"User logged out: {username}");
            
            return userExists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout");
            throw;
        }
    }
}