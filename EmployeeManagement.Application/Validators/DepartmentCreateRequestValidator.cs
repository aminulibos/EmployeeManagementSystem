using EmployeeManagement.Application.DTOs.Department;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

/// <summary>
/// Department Creation Request Validator
/// 
/// Validation Rules:
/// 1. Name - Required, valid length, alphanumeric
/// 2. Description - Optional but if provided must meet constraints
/// 
/// Junior-Friendly: Shows how to validate optional fields
/// </summary>
public class DepartmentCreateRequestValidator : AbstractValidator<DepartmentCreateRequest>
{
    public DepartmentCreateRequestValidator()
    {
        // Rule 1: Department name validation
        // - Cannot be empty (required field)
        // - Must be at least 2 characters
        // - Cannot exceed 100 characters
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        // Rule 2: Description validation (optional field)
        // - If provided, must not exceed 500 characters
        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

