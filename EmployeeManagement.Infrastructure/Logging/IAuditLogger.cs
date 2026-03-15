using EmployeeManagement.Persistence.Data;

namespace EmployeeManagement.Infrastructure.Logging;

public interface IAuditLogger
{
    Task LogAsync(string userId, string action, string resource, string details, bool isSuccessful = true);
    Task<IEnumerable<AuditLog>> GetLogsAsync(DateTime from, DateTime to, string? userId = null, string? resource = null);
}


