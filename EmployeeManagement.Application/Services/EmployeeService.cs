using EmployeeManagement.Application.DTOs.Employee;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data.Repositories;
using EmployeeManagement.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(IUnitOfWork unitOfWork, IAuditLogger auditLogger, ILogger<EmployeeService> logger)
    {
        _unitOfWork = unitOfWork;
        _auditLogger = auditLogger;
        _logger = logger;
    }

    public async Task<EmployeeResponse> CreateEmployeeAsync(EmployeeCreateRequest request)
    {
        try
        {
            var deptExists = await _unitOfWork.Departments.AnyAsync(d => d.Id == request.DepartmentId).ConfigureAwait(false);
            if (!deptExists)
                throw new ArgumentException("Department not found");

            var desigExists = await _unitOfWork.Designations.AnyAsync(d => d.Id == request.DesignationId).ConfigureAwait(false);
            if (!desigExists)
                throw new ArgumentException("Designation not found");

            var employee = new Employee
            {
                Name = request.Name,
                Email = request.Email,
                Phone = request.Phone,
                DepartmentId = request.DepartmentId,
                DesignationId = request.DesignationId,
                IsActive = true,
                JoiningDate = DateTime.UtcNow
            };

            await _unitOfWork.Employees.AddAsync(employee).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            await _auditLogger.LogAsync("system", "CREATE", "Employee", $"Created employee: {employee.Name}", true).ConfigureAwait(false);

            _logger.LogInformation($"Employee created successfully: {employee.Name}");
            return new EmployeeResponse { Id = employee.Id, Name = employee.Name, Email = employee.Email, Phone = employee.Phone };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee");
            await _auditLogger.LogAsync("system", "CREATE", "Employee", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync()
    {
        try
        {
            var employees = await _unitOfWork.Employees.GetAllAsync().ConfigureAwait(false);
            return employees.Select(e => new EmployeeResponse
            {
                Id = e.Id,
                Name = e.Name,
                Email = e.Email,
                Phone = e.Phone
            }).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employees");
            throw;
        }
    }

    public async Task<EmployeeResponse?> GetEmployeeByIdAsync(int id)
    {
        try
        {
            var emp = await _unitOfWork.Employees.GetByIdAsync(id).ConfigureAwait(false);
            if (emp == null)
                return null;

            return new EmployeeResponse { Id = emp.Id, Name = emp.Name, Email = emp.Email, Phone = emp.Phone };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee");
            throw;
        }
    }

    public async Task<EmployeeResponse?> UpdateEmployeeAsync(int id, EmployeeCreateRequest request)
    {
        try
        {
            var emp = await _unitOfWork.Employees.GetByIdAsync(id).ConfigureAwait(false);
            if (emp == null)
                return null;

            emp.Name = request.Name;
            emp.Email = request.Email;
            emp.Phone = request.Phone;

            _unitOfWork.Employees.Update(emp);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            await _auditLogger.LogAsync("system", "UPDATE", "Employee", $"Updated employee: {emp.Name}", true).ConfigureAwait(false);

            _logger.LogInformation($"Employee updated successfully: {emp.Name}");
            return new EmployeeResponse { Id = emp.Id, Name = emp.Name, Email = emp.Email, Phone = emp.Phone };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee");
            await _auditLogger.LogAsync("system", "UPDATE", "Employee", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<bool> DeactivateEmployeeAsync(int id)
    {
        try
        {
            var emp = await _unitOfWork.Employees.GetByIdAsync(id).ConfigureAwait(false);
            if (emp == null)
                return false;

            emp.IsActive = false;
            _unitOfWork.Employees.Update(emp);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);

            await _auditLogger.LogAsync("system", "DEACTIVATE", "Employee", $"Deactivated employee: {emp.Name}", true).ConfigureAwait(false);

            _logger.LogInformation($"Employee deactivated successfully: {emp.Name}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating employee");
            await _auditLogger.LogAsync("system", "DEACTIVATE", "Employee", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }
}
