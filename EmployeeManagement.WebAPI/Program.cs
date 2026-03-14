using EmployeeManagement.Configuration;
using EmployeeManagement.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection Setup
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddEnvironmentConfiguration(builder.Configuration);

var app = builder.Build();

// Database Initialization
await DatabaseInitialization.InitializeDatabasesAsync(app);

// Middleware Configuration
app
    .UseApplicationMiddleware(app.Environment)
    .UseMiddleware<PermissionMiddleware>();

app.Run();
