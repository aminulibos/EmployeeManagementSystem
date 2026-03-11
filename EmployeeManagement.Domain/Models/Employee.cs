using EmployeeManagement.Domain.Common;
using EmployeeManagement.Domain.Enums;

namespace EmployeeManagement.Domain.Models;

public class Employee : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public GenderEnum Gender { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid DesignationId { get; set; }
    public bool IsActive { get; set; } = true;
}