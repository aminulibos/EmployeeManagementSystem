using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models;

public class Permission : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}