using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Configuration;

public static class EnvironmentConfiguration
{
    public static IServiceCollection AddEnvironmentConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<LoggingSettings>(configuration.GetSection("LoggingSettings"));

        ValidateConfiguration(configuration);

        return services;
    }

    private static void ValidateConfiguration(IConfiguration configuration)
    {
        var errors = new List<string>();

        if (string.IsNullOrEmpty(configuration["JwtSettings:SecretKey"]))
            errors.Add("JwtSettings:SecretKey is not configured");

        if (string.IsNullOrEmpty(configuration["JwtSettings:Issuer"]))
            errors.Add("JwtSettings:Issuer is not configured");

        if (string.IsNullOrEmpty(configuration["JwtSettings:Audience"]))
            errors.Add("JwtSettings:Audience is not configured");

        if (string.IsNullOrEmpty(configuration.GetConnectionString("DefaultConnection")))
            errors.Add("DefaultConnection (MSSQL) is not configured");

        if (errors.Count > 0)
        {
            Console.WriteLine("CONFIGURATION VALIDATION ERRORS:");
            foreach (var error in errors)
            {
                Console.WriteLine($"   - {error}");
            }
            throw new InvalidOperationException("Configuration validation failed. See above for details.");
        }

        Console.WriteLine("Configuration validation passed");
    }
}

public class JwtSettings
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpirationMinutes { get; set; } = 60;
}

public class DatabaseSettings
{
    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public bool EnableQueryLogging { get; set; } = false;
}

public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SenderEmail { get; set; } = string.Empty;
    public string SenderPassword { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
}

public class LoggingSettings
{
    public string LogLevel { get; set; } = "Information";
    public bool EnableConsoleLogging { get; set; } = true;
    public bool EnableFileLogging { get; set; } = true;
    public string LogFilePath { get; set; } = "logs/";
    public bool EnableDatabaseQueryLogging { get; set; } = false;
}

