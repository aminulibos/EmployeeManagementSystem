using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Seeders;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        try
        {
            if (await db.Roles.AnyAsync()) return;

            var permissions = new List<Permission>
            {
                new() { Name = "roles.read" },
                new() { Name = "roles.write" },
                new() { Name = "employees.read" },
                new() { Name = "employees.write" },
                new() { Name = "departments.read" },
                new() { Name = "departments.write" },
                new() { Name = "designations.read" },
                new() { Name = "designations.write" },
                new() { Name = "salary.read" },
                new() { Name = "salary.write" },
                new() { Name = "reports.view" }
            };

            await db.Permissions.AddRangeAsync(permissions);
            await db.SaveChangesAsync();

            var adminRole = new Role { Name = "Admin" };
            var hrRole = new Role { Name = "HR" };
            var viewerRole = new Role { Name = "Viewer" };

            await db.Roles.AddRangeAsync(adminRole, hrRole, viewerRole);
            await db.SaveChangesAsync();

            var adminPerms = permissions.Select(p => new RolePermission { RoleId = adminRole.Id, PermissionId = p.Id }).ToList();
            var hrPerms = permissions.Where(p => p.Name.Contains("read") || p.Name.Contains("write") || p.Name == "reports.view")
                .Select(p => new RolePermission { RoleId = hrRole.Id, PermissionId = p.Id }).ToList();
            var viewerPerms = permissions.Where(p => p.Name.Contains("read"))
                .Select(p => new RolePermission { RoleId = viewerRole.Id, PermissionId = p.Id }).ToList();

            var allPerms = new List<RolePermission>();
            allPerms.AddRange(adminPerms);
            allPerms.AddRange(hrPerms);
            allPerms.AddRange(viewerPerms);

            await db.RolePermissions.AddRangeAsync(allPerms);
            await db.SaveChangesAsync();

            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                RoleId = adminRole.Id
            };

            await db.Users.AddAsync(adminUser);
            await db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Seed error: {ex.Message}");
        }
    }
}

