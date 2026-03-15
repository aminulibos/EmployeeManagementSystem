using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class AuditService : IAuditService
{
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<AuditService> _logger;

    public AuditService(IAuditLogger auditLogger, ILogger<AuditService> logger)
    {
        _auditLogger = auditLogger;
        _logger = logger;
    }

    public async Task LogActionAsync(string action, string entityType, string? details, string? username)
    {
        try
        {
            var userId = username ?? "system";
            await _auditLogger.LogAsync(userId, action, entityType, details ?? string.Empty, true).ConfigureAwait(false);
            _logger.LogInformation($"Audit logged: {action} on {entityType}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging audit action");
        }
    }
}

