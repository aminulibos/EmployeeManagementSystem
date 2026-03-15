using EmployeeManagement.Application.DTOs.Auth;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

/// <summary>
/// Login Request Validator
/// 
/// Validation Rules:
/// 1. Username - Required, valid length
/// 2. Password - Required, minimum security length
/// 
/// Junior-Friendly Fluent Interface:
/// - NotEmpty() = Field must have a value
/// - MinimumLength() = String must meet minimum length
/// - MaximumLength() = String must not exceed maximum
/// - WithMessage() = User-friendly error message
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        // Rule 1: Username validation
        // - Cannot be empty
        // - Must be 3-50 characters (business constraint)
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Username is required")
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters")
            .MaximumLength(50)
            .WithMessage("Username cannot exceed 50 characters");

        // Rule 2: Password validation
        // - Cannot be empty
        // - Must be at least 6 characters (security requirement)
        // - Cannot exceed 100 characters (reasonable limit)
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters")
            .MaximumLength(100)
            .WithMessage("Password cannot exceed 100 characters");
    }
}

