namespace EmployeeManagement.Application.Interfaces;

public interface ILoggingService
{
    Task LogAsync(string action, string details);
}

