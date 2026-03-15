﻿using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Response;
using EmployeeManagement.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportingController : ControllerBase
{
    private readonly IReportingService _service;

    public ReportingController(IReportingService service) => _service = service;

    [HttpGet("total-salary")]
    [RequirePermission("reports.view")]
    public async Task<IActionResult> GetTotalSalary()
    {
        try
        {
            var data = await _service.GetTotalSalaryDisbursedAsync();
            return Ok(ApiResponse<object>.Success(data, "Total salary fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpGet("distribution")]
    [RequirePermission("reports.view")]
    public async Task<IActionResult> GetDistribution()
    {
        try
        {
            var data = await _service.GetSalaryDistributionByDepartmentAsync();
            return Ok(ApiResponse<object>.Success(data, "Salary distribution fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpGet("monthly/{month}/{year}")]
    [RequirePermission("reports.view")]
    public async Task<IActionResult> GetMonthly(int month, int year)
    {
        try
        {
            var data = await _service.GetMonthlySalaryAsync(month, year);
            return Ok(ApiResponse<object>.Success(data, "Monthly salary fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpGet("employee-history/{employeeId}")]
    [RequirePermission("reports.view")]
    public async Task<IActionResult> GetEmployeeHistory(int employeeId)
    {
        try
        {
            var data = await _service.GetEmployeeSalaryHistoryAsync(employeeId);
            return Ok(ApiResponse<object>.Success(data, "Employee history fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }
}


