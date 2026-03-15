using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Infrastructure.Logging;

public interface IApplicationLogger
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message, Exception? exception = null);
    void LogDebug(string message);
}

public class ApplicationLogger(ILogger<ApplicationLogger> logger) : IApplicationLogger
{
    private readonly ILogger<ApplicationLogger> _logger = logger;

    public void LogInformation(string message)
    {
        _logger.LogInformation($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}");
    }

    public void LogWarning(string message)
    {
        _logger.LogWarning($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}");
    }

    public void LogError(string message, Exception? exception = null)
    {
        if (exception != null)
            _logger.LogError(exception, $"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}");
        else
            _logger.LogError($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}");
    }

    public void LogDebug(string message)
    {
        _logger.LogDebug($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] {message}");
    }
}

