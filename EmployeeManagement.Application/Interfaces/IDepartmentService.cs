﻿using EmployeeManagement.Application.DTOs.Department;

namespace EmployeeManagement.Application.Interfaces;

public interface IDepartmentService
{
    Task<DepartmentResponse> CreateDepartmentAsync(DepartmentCreateRequest request);
    Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
    Task<DepartmentResponse?> GetDepartmentByIdAsync(int id);
    Task<DepartmentResponse?> UpdateDepartmentAsync(int id, DepartmentCreateRequest request);
    Task<bool> DeleteDepartmentAsync(int id);
}