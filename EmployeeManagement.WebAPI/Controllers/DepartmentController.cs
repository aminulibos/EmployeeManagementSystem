﻿﻿using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Response;
using EmployeeManagement.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentController(IDepartmentService service) : ControllerBase
{
    [HttpPost]
    [RequirePermission("departments.write")]
    public async Task<IActionResult> Create([FromBody] DepartmentCreateRequest request)
    {
        try
        {
            var department = await service.CreateDepartmentAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, 
                ApiResponse<object>.Success(department, "Department created", "201"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Create failed", "400"));
        }
    }

    [HttpGet]
    [RequirePermission("departments.read")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var departments = await service.GetAllDepartmentsAsync();
            return Ok(ApiResponse<object>.Success(departments, "Departments fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpGet("{id:int}")]
    [RequirePermission("departments.read")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var department = await service.GetDepartmentByIdAsync(id);
            if (department is null)
                return NotFound(ApiResponse<object>.Failure("Department not found", "404"));
            return Ok(ApiResponse<object>.Success(department, "Department fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpPut("{id:int}")]
    [RequirePermission("departments.write")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentCreateRequest request)
    {
        try
        {
            var updated = await service.UpdateDepartmentAsync(id, request);
            if (updated is null)
                return NotFound(ApiResponse<object>.Failure("Department not found", "404"));
            return Ok(ApiResponse<object>.Success(updated, "Department updated"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Update failed", "400"));
        }
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("departments.write")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await service.DeleteDepartmentAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.Failure("Department not found", "404"));
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Delete failed", "400"));
        }
    }
}