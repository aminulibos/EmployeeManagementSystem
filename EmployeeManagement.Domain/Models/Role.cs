using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}