using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Response;
using EmployeeManagement.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController(IEmployeeService service) : ControllerBase
{
    [HttpPost]
    [RequirePermission("employees.write")]
    public async Task<IActionResult> Create([FromBody] EmployeeCreateRequest request)
    {
        try
        {
            var employee = await service.CreateEmployeeAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, 
                ApiResponse<object>.Success(employee, "Employee created", "201"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure(ex.Message, "400"));
        }
    }

    [HttpGet]
    [RequirePermission("employees.read")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var employees = await service.GetAllEmployeesAsync();
            return Ok(ApiResponse<object>.Success(employees, "Employees fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure(ex.Message, "400"));
        }
    }

    [HttpGet("{id}")]
    [RequirePermission("employees.read")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var employee = await service.GetEmployeeByIdAsync(id);
            if (employee is null)
                return NotFound(ApiResponse<object>.Failure("Employee not found", "404"));
            return Ok(ApiResponse<object>.Success(employee, "Employee fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Failed to fetch employee", "400"));
        }
    }

    [HttpPut("{id}")]
    [RequirePermission("employees.write")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeCreateRequest request)
    {
        try
        {
            var employee = await service.UpdateEmployeeAsync(id, request);
            if (employee is null)
                return NotFound(ApiResponse<object>.Failure("Employee not found", "404"));
            return Ok(ApiResponse<object>.Success(employee, "Employee updated"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Update failed", "400"));
        }
    }

    [HttpPut("{id:int}/deactivate")]
    [RequirePermission("employees.write")]
    public async Task<IActionResult> Deactivate(int id)
    {
        try
        {
            var success = await service.DeactivateEmployeeAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.Failure("Employee not found", "404"));
            return Ok(ApiResponse<object>.Success(null, "Employee deactivated"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Deactivation failed", "400"));
        }
    }
}