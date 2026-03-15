using EmployeeManagement.Application.DTOs.Employee;

namespace EmployeeManagement.Application.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeResponse> CreateEmployeeAsync(EmployeeCreateRequest request);
    Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync();
    Task<EmployeeResponse?> GetEmployeeByIdAsync(int id);
    Task<EmployeeResponse?> UpdateEmployeeAsync(int id, EmployeeCreateRequest request);
    Task<bool> DeactivateEmployeeAsync(int id);
}

