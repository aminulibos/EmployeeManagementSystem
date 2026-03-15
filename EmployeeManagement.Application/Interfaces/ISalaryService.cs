namespace EmployeeManagement.Application.Interfaces;

public interface ISalaryService
{
    Task<object> GetSalaryHistoryAsync(int employeeId);
    Task<object> DisburseAsync(int employeeId, decimal amount, int month, int year);
    Task<object> GetDepartmentSalaryAsync(int departmentId);
}

