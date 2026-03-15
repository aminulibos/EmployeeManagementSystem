﻿using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data.Repositories;
using EmployeeManagement.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class SalaryService : ISalaryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<SalaryService> _logger;

    public SalaryService(IUnitOfWork unitOfWork, IAuditLogger auditLogger, ILogger<SalaryService> logger)
    {
        _unitOfWork = unitOfWork;
        _auditLogger = auditLogger;
        _logger = logger;
    }

    public async Task<object> GetSalaryHistoryAsync(int employeeId)
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.FindAsync(s => s.EmployeeId == employeeId).ConfigureAwait(false);
            return salaries
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .Select(s => new { s.Id, s.Month, s.Year, s.Amount, s.DisbursedDate })
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving salary history");
            throw;
        }
    }

    public async Task<object> DisburseAsync(int employeeId, decimal amount, int month, int year)
    {
        try
        {
            if (amount <= 0)
                throw new ArgumentException("Salary amount must be greater than 0");

            var employee = await _unitOfWork.Employees.GetByIdAsync(employeeId).ConfigureAwait(false);
            if (employee == null)
                throw new ArgumentException("Employee not found");

            var existing = await _unitOfWork.Salaries.FirstOrDefaultAsync(s => 
                s.EmployeeId == employeeId && s.Month == month && s.Year == year).ConfigureAwait(false);

            if (existing != null)
            {
                existing.Amount = amount;
                existing.DisbursedDate = DateTime.UtcNow;
                _unitOfWork.Salaries.Update(existing);
            }
            else
            {
                var salary = new Salary
                {
                    EmployeeId = employeeId,
                    Amount = amount,
                    Month = month,
                    Year = year,
                    DisbursedDate = DateTime.UtcNow
                };
                await _unitOfWork.Salaries.AddAsync(salary).ConfigureAwait(false);
            }

            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            await _auditLogger.LogAsync("system", "DISBURSE", "Salary", 
                $"Salary disbursed to employee {employeeId}: {amount}", true).ConfigureAwait(false);

            _logger.LogInformation($"Salary disbursed successfully for employee {employeeId}");
            return new { message = "Salary disbursed successfully" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error disbursing salary");
            await _auditLogger.LogAsync("system", "DISBURSE", "Salary", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<object> GetDepartmentSalaryAsync(int departmentId)
    {
        try
        {
            var salaries = await _unitOfWork.Salaries.GetAllAsync().ConfigureAwait(false);
            return salaries
                .Where(s => s.Employee?.DepartmentId == departmentId)
                .GroupBy(s => new { s.Month, s.Year })
                .Select(g => new 
                { 
                    Month = g.Key.Month, 
                    Year = g.Key.Year, 
                    TotalAmount = g.Sum(s => s.Amount), 
                    Count = g.Count() 
                })
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department salary");
            throw;
        }
    }
}
