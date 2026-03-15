using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Application.DTOs.Salary;
using EmployeeManagement.Shared.Response;
using EmployeeManagement.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _service;

    public SalaryController(ISalaryService service) => _service = service;

    [HttpGet("history/{employeeId}")]
    [RequirePermission("salary.read")]
    public async Task<IActionResult> GetHistory(int employeeId)
    {
        try
        {
            var data = await _service.GetSalaryHistoryAsync(employeeId);
            return Ok(ApiResponse<object>.Success(data, "Salary history fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpPost("disburse")]
    [RequirePermission("salary.write")]
    public async Task<IActionResult> Disburse([FromBody] DisburseRequest request)
    {
        try
        {
            var result = await _service.DisburseAsync(request.EmployeeId, request.Amount, request.Month, request.Year);
            return Ok(ApiResponse<object>.Success(result, "Salary disbursed"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Disburse failed", "400"));
        }
    }

    [HttpGet("department/{departmentId}")]
    [RequirePermission("salary.read")]
    public async Task<IActionResult> GetDepartmentSalary(int departmentId)
    {
        try
        {
            var data = await _service.GetDepartmentSalaryAsync(departmentId);
            return Ok(ApiResponse<object>.Success(data, "Department salary fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }
}
