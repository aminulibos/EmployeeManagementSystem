using EmployeeManagement.Application.DTOs.Designation;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

/// <summary>
/// Designation Creation Request Validator
/// 
/// Validation Rules:
/// 1. Title - Required, valid length
/// 2. Description - Optional with length constraints
/// 
/// Junior-Friendly: Shows validation of job titles/positions
/// </summary>
public class DesignationCreateRequestValidator : AbstractValidator<DesignationCreateRequest>
{
    public DesignationCreateRequestValidator()
    {
        // Rule 1: Designation title validation
        // - Cannot be empty (required field)
        // - Must be at least 2 characters
        // - Cannot exceed 100 characters
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Designation title is required")
            .MinimumLength(2)
            .WithMessage("Title must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("Title cannot exceed 100 characters");

        // Rule 2: Description validation (optional field)
        // - If provided, must not exceed 500 characters
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

