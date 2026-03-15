namespace EmployeeManagement.Application.Interfaces;

public interface IAuditService
{
    Task LogActionAsync(string action, string entityType, string? details, string? username);
}

