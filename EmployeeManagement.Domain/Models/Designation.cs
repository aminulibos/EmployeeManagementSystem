using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Models;

public class Designation : BaseEntity
{
    public string Title { get; set; } = string.Empty;
}