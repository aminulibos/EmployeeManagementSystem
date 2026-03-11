using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}