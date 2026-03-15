﻿﻿using EmployeeManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmployeeManagement.WebAPI.Middleware;

public class PermissionMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, AppDbContext db)
    {
        var endpoint = context.GetEndpoint();
        var permissionAttr = endpoint?.Metadata.GetMetadata<Attributes.RequirePermissionAttribute>();

        if (permissionAttr is null)
        {
            await next(context);
            return;
        }

        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(
                new { isSuccess = false, statusCode = "401", message = "Unauthorized" });
            return;
        }

        var username = context.User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(username))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(
                new { isSuccess = false, statusCode = "401", message = "Unauthorized" });
            return;
        }

        var user = await db.Users
            .Include(u => u.Role)
            .ThenInclude(r => r!.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(u => u.Username == username);

        if (user is null)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(
                new { isSuccess = false, statusCode = "401", message = "User not found" });
            return;
        }

        var hasPermission = user.Role?.RolePermissions?
            .Any(rp => rp.Permission?.Name == permissionAttr.Permission) ?? false;

        if (!hasPermission)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsJsonAsync(
                new { isSuccess = false, statusCode = "403", message = "Forbidden - insufficient permissions" });
            return;
        }

        await next(context);
    }
}

