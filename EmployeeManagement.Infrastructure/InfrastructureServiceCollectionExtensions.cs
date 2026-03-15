using EmployeeManagement.Infrastructure.Authentication;
using EmployeeManagement.Infrastructure.Caching;
using EmployeeManagement.Infrastructure.Data.Repositories;
using EmployeeManagement.Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagement.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        AddRepositories(services);
        AddAuthentication(services);
        AddCaching(services);
        AddLogging(services);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
    }

    private static void AddCaching(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddScoped<ICacheService, MemoryCacheService>();
    }

    private static void AddLogging(IServiceCollection services)
    {
        services.AddScoped<IApplicationLogger, ApplicationLogger>();
        services.AddScoped<IAuditLogger, DatabaseAuditLogger>();
    }
}
