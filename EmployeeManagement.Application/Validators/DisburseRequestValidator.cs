using FluentValidation;
using EmployeeManagement.Application.DTOs.Salary;

namespace EmployeeManagement.Application.Validators;

/// <summary>
/// Validator for Salary Disbursement Requests
/// 
/// Validation Rules:
/// - EmployeeId must be greater than 0 (valid employee reference)
/// - Amount must be greater than 0 (positive salary amount)
/// - Month must be between 1-12 (valid calendar month)
/// - Year must be current year or future year (reasonable date range)
/// 
/// Junior-Friendly: Uses fluent interface for readable validation chains
/// </summary>
public class DisburseRequestValidator : AbstractValidator<DisburseRequest>
{
    public DisburseRequestValidator()
    {
        // Employee ID validation
        RuleFor(x => x.EmployeeId)
            .GreaterThan(0)
            .WithMessage("Valid employee ID is required");

        // Amount validation
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Salary amount must be greater than 0")
            .LessThanOrEqualTo(9999999.99m)
            .WithMessage("Salary amount cannot exceed maximum limit");

        // Month validation
        RuleFor(x => x.Month)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Month must be between 1 and 12")
            .LessThanOrEqualTo(12)
            .WithMessage("Month must be between 1 and 12");

        // Year validation
        RuleFor(x => x.Year)
            .GreaterThanOrEqualTo(2020)
            .WithMessage("Year must be 2020 or later")
            .LessThanOrEqualTo(2100)
            .WithMessage("Year must be reasonable (2100 or earlier)");
    }
}
