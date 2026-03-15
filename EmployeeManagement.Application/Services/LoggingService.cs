using EmployeeManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class LoggingService : ILoggingService
{
    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger) => _logger = logger;

    public Task LogAsync(string action, string details)
    {
        _logger.LogInformation($"[{DateTime.UtcNow}] Action: {action}, Details: {details}");
        return Task.CompletedTask;
    }
}
