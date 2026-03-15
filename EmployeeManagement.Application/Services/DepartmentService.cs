using EmployeeManagement.Application.DTOs.Department;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data.Repositories;
using EmployeeManagement.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IUnitOfWork unitOfWork, IAuditLogger auditLogger, ILogger<DepartmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _auditLogger = auditLogger;
        _logger = logger;
    }

    public async Task<DepartmentResponse> CreateDepartmentAsync(DepartmentCreateRequest request)
    {
        try
        {
            var department = new Department
            {
                Name = request.Name,
                Description = request.Description
            };

            await _unitOfWork.Departments.AddAsync(department).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await _auditLogger.LogAsync("system", "CREATE", "Department", $"Created department: {department.Name}", true).ConfigureAwait(false);
            _logger.LogInformation($"Department created successfully: {department.Name}");
            return new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            await _auditLogger.LogAsync("system", "CREATE", "Department", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
    {
        try
        {
            var departments = await _unitOfWork.Departments.GetAllAsync().ConfigureAwait(false);
            return departments.Select(d => new DepartmentResponse
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving departments");
            throw;
        }
    }

    public async Task<DepartmentResponse?> GetDepartmentByIdAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id).ConfigureAwait(false);
            if (department == null)
                return null;

            return new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving department");
            throw;
        }
    }

    public async Task<DepartmentResponse?> UpdateDepartmentAsync(int id, DepartmentCreateRequest request)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id).ConfigureAwait(false);
            if (department == null)
                return null;

            department.Name = request.Name;
            department.Description = request.Description;

            _unitOfWork.Departments.Update(department);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await _auditLogger.LogAsync("system", "UPDATE", "Department", $"Updated department: {department.Name}", true).ConfigureAwait(false);
            _logger.LogInformation($"Department updated successfully: {department.Name}");
            return new DepartmentResponse
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating department");
            await _auditLogger.LogAsync("system", "UPDATE", "Department", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.Departments.GetByIdAsync(id).ConfigureAwait(false);
            if (department == null)
                return false;

            _unitOfWork.Departments.Remove(department);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await _auditLogger.LogAsync("system", "DELETE", "Department", $"Deleted department: {department.Name}", true).ConfigureAwait(false);
            _logger.LogInformation($"Department deleted successfully: {department.Name}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting department");
            await _auditLogger.LogAsync("system", "DELETE", "Department", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }
}