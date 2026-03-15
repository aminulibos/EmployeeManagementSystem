using EmployeeManagement.Application.DTOs.Employee;
using FluentValidation;

namespace EmployeeManagement.Application.Validators;

/// <summary>
/// Employee Creation Request Validator
/// 
/// Validation Rules:
/// 1. Name - Required, minimum 2 characters
/// 2. Email - Required, valid email format
/// 3. Phone - Required, valid phone format
/// 4. DepartmentId - Required, must be valid (>0)
/// 5. DesignationId - Required, must be valid (>0)
/// 
/// Junior-Friendly Fluent Interface:
/// - RuleFor() = Specify which property to validate
/// - NotEmpty() = Property must have a value
/// - EmailAddress() = Property must be valid email
/// - MinimumLength() = String must be long enough
/// - GreaterThan(0) = ID references must be valid
/// - WithMessage() = Friendly error message if validation fails
/// </summary>
public class EmployeeCreateRequestValidator : AbstractValidator<EmployeeCreateRequest>
{
    public EmployeeCreateRequestValidator()
    {
        // Rule 1: Name validation
        // - Cannot be empty
        // - Must be at least 2 characters
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Employee name is required")
            .MinimumLength(2)
            .WithMessage("Name must be at least 2 characters")
            .MaximumLength(100)
            .WithMessage("Name cannot exceed 100 characters");

        // Rule 2: Email validation
        // - Cannot be empty
        // - Must be valid email format (business rule: email uniqueness checked in service)
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email format is invalid");

        // Rule 3: Phone validation
        // - Cannot be empty
        // - Must be numeric (10+ digits typical for phone numbers)
        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^\d{10,}$")
            .WithMessage("Phone number must be at least 10 digits");

        // Rule 4: Department reference validation
        // - Must be greater than 0 (valid foreign key reference)
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("Valid department is required");

        // Rule 5: Designation reference validation
        // - Must be greater than 0 (valid foreign key reference)
        RuleFor(x => x.DesignationId)
            .GreaterThan(0)
            .WithMessage("Valid designation is required");
    }
}

