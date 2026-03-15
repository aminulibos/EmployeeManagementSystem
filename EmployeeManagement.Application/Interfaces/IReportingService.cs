namespace EmployeeManagement.Application.Interfaces;

public interface IReportingService
{
    Task<object> GetTotalSalaryDisbursedAsync();
    Task<object> GetSalaryDistributionByDepartmentAsync();
    Task<object> GetMonthlySalaryAsync(int month, int year);
    Task<object> GetEmployeeSalaryHistoryAsync(int employeeId);
}

