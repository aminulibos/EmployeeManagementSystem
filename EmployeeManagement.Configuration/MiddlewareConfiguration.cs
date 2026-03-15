using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using EmployeeManagement.Persistence.Data;

namespace EmployeeManagement.Configuration;

public static class MiddlewareConfiguration
{
    public static WebApplication UseApplicationMiddleware(
        this WebApplication app,
        IHostEnvironment env)
    {
        app.UseCors("AllowAll");

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API v1");
            });
        }

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        MapHealthCheckEndpoints(app);

        return app;
    }

    private static void MapHealthCheckEndpoints(WebApplication app)
    {
        app.MapGet("/", () =>
            Results.Ok(new
            {
                status = "UP",
                uptime = DateTime.UtcNow
            }))
            .WithName("HealthCheck")
            .AllowAnonymous();

        app.MapGet("/detail", async (AppDbContext dbContext) =>
        {
            try
            {
                var canConnect = await dbContext.Database.CanConnectAsync();
                if (canConnect)
                {
                    return Results.Ok(new
                    {
                        status = "UP",
                        database = "connected",
                        timestamp = DateTime.UtcNow
                    });
                }
                else
                {
                    return Results.StatusCode(503);
                }
            }
            catch
            {
                return Results.StatusCode(503);
            }
        })
            .WithName("HealthCheckDetailed")
            .AllowAnonymous();
    }
}

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new
        {
            success = false,
            message = "An unexpected error occurred",
            details = exception.Message,
            timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}




