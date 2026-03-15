using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Persistence.Data;

public class LogDbContext : DbContext
{
    public LogDbContext(DbContextOptions<LogDbContext> options) : base(options) { }

    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Action).IsRequired();
            entity.Property(e => e.Resource).IsRequired();
            entity.Property(e => e.Details);
            entity.Property(e => e.IsSuccessful).IsRequired();
            entity.Property(e => e.Timestamp).IsRequired();
            
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Resource);
        });

        base.OnModelCreating(modelBuilder);
    }
}

public class AuditLog
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Resource { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public bool IsSuccessful { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

