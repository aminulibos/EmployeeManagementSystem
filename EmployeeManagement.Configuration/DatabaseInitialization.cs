using EmployeeManagement.Persistence.Data;
using EmployeeManagement.Persistence.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Configuration;

public static class DatabaseInitialization
{
    public static async Task InitializeDatabasesAsync(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                await InitializeMssqlAsync(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSSQL initialization error: {ex.Message}");
            }

            try
            {
                await InitializePostgresqlAsync(services);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PostgreSQL initialization error: {ex.Message}");
                Console.WriteLine("System will continue without PostgreSQL logging");
            }
        }
    }

    private static async Task InitializeMssqlAsync(IServiceProvider services)
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
        Console.WriteLine("MSSQL migrations applied successfully");

        await DbSeeder.SeedAsync(dbContext);
        Console.WriteLine("MSSQL database seeded successfully");
    }

    private static async Task InitializePostgresqlAsync(IServiceProvider services)
    {
        try
        {
            var logDbContext = services.GetRequiredService<LogDbContext>();
            await logDbContext.Database.MigrateAsync();
            Console.WriteLine("PostgreSQL migrations applied successfully");
        }
        catch (InvalidOperationException)
        {
            Console.WriteLine("PostgreSQL not configured, skipping initialization");
        }
    }
}



