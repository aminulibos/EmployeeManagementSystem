﻿using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Infrastructure.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class ReportingService : IReportingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReportingService> _logger;

    public ReportingService(IUnitOfWork unitOfWork, ILogger<ReportingService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<object> GetTotalSalaryDisbursedAsync()
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.GetAllAsync().ConfigureAwait(false);
            var total = salaries.Sum(s => s.Amount);
            var count = salaries.Count();
            return new { TotalAmount = total, TransactionCount = count };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving total salary disbursed");
            throw;
        }
    }

    public async Task<object> GetSalaryDistributionByDepartmentAsync()
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.GetAllAsync().ConfigureAwait(false);
            return salaries
                .GroupBy(s => s.Employee?.Department?.Name ?? "Unknown")
                .Select(g => new
                {
                    Department = g.Key,
                    TotalAmount = g.Sum(s => s.Amount),
                    EmployeeCount = g.Select(s => s.EmployeeId).Distinct().Count(),
                    AverageAmount = g.Average(s => s.Amount)
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving salary distribution by department");
            throw;
        }
    }

    public async Task<object> GetMonthlySalaryAsync(int month, int year)
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.FindAsync(s => s.Month == month && s.Year == year).ConfigureAwait(false);
            return salaries
                .GroupBy(s => s.Employee?.Department?.Name ?? "Unknown")
                .Select(g => new 
                { 
                    Department = g.Key, 
                    TotalAmount = g.Sum(s => s.Amount), 
                    EmployeeCount = g.Count() 
                })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving monthly salary");
            throw;
        }
    }

    public async Task<object> GetEmployeeSalaryHistoryAsync(int employeeId)
    {
        try
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId).ConfigureAwait(false);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            var salaries = await _unitOfWork.Salaries.FindAsync(s => s.EmployeeId == employeeId).ConfigureAwait(false);
            var salaryList = salaries
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .Select(s => new { Month = s.Month, Year = s.Year, Amount = s.Amount, DisbursedDate = s.DisbursedDate })
                .ToList();

            return new
            {
                Employee = new 
                { 
                    employee.Id, 
                    employee.Name, 
                    employee.Email, 
                    Department = employee.Department?.Name ?? "Unknown" 
                },
                Salaries = salaryList,
                TotalAmount = salaryList.Sum(s => (decimal)s.Amount)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee salary history");
            throw;
        }
    }
}
