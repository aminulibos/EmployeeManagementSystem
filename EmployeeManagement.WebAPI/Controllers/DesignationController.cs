﻿﻿using EmployeeManagement.Application.DTOs.Designation;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Shared.Response;
using EmployeeManagement.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DesignationController(IDesignationService service) : ControllerBase
{
    [HttpPost]
    [RequirePermission("designations.write")]
    public async Task<IActionResult> Create([FromBody] DesignationCreateRequest request)
    {
        try
        {
            var designation = await service.CreateDesignationAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = designation.Id }, 
                ApiResponse<object>.Success(designation, "Designation created", "201"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Create failed", "400"));
        }
    }

    [HttpGet]
    [RequirePermission("designations.read")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var designations = await service.GetAllDesignationsAsync();
            return Ok(ApiResponse<object>.Success(designations, "Designations fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpGet("{id:int}")]
    [RequirePermission("designations.read")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var designation = await service.GetDesignationByIdAsync(id);
            if (designation is null)
                return NotFound(ApiResponse<object>.Failure("Designation not found", "404"));
            return Ok(ApiResponse<object>.Success(designation, "Designation fetched"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Fetch failed", "400"));
        }
    }

    [HttpPut("{id:int}")]
    [RequirePermission("designations.write")]
    public async Task<IActionResult> Update(int id, [FromBody] DesignationCreateRequest request)
    {
        try
        {
            var updated = await service.UpdateDesignationAsync(id, request);
            if (updated is null)
                return NotFound(ApiResponse<object>.Failure("Designation not found", "404"));
            return Ok(ApiResponse<object>.Success(updated, "Designation updated"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Update failed", "400"));
        }
    }

    [HttpDelete("{id:int}")]
    [RequirePermission("designations.write")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var success = await service.DeleteDesignationAsync(id);
            if (!success)
                return NotFound(ApiResponse<object>.Failure("Designation not found", "404"));
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.Failure("Delete failed", "400"));
        }
    }
}