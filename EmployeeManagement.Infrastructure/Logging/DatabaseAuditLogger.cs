using EmployeeManagement.Infrastructure.Data.Repositories;
using EmployeeManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Infrastructure.Logging;

public class DatabaseAuditLogger(LogDbContext logContext, ILogger<DatabaseAuditLogger> logger) : IAuditLogger
{
    private readonly LogDbContext _logContext = logContext;
    private readonly ILogger<DatabaseAuditLogger> _logger = logger;

    public async Task LogAsync(string userId, string action, string resource, string details, bool isSuccessful = true)
    {
        try
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                Resource = resource,
                Details = details,
                IsSuccessful = isSuccessful,
                Timestamp = DateTime.UtcNow
            };

            _logContext.AuditLogs.Add(auditLog);
            await _logContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error logging audit trail: {ex.Message}");
        }
    }

    public async Task<IEnumerable<AuditLog>> GetLogsAsync(DateTime from, DateTime to, string? userId = null, string? resource = null)
    {
        var query = _logContext.AuditLogs
            .Where(al => al.Timestamp >= from && al.Timestamp <= to);

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(al => al.UserId == userId);

        if (!string.IsNullOrEmpty(resource))
            query = query.Where(al => al.Resource == resource);

        return await query.ToListAsync();
    }
}

