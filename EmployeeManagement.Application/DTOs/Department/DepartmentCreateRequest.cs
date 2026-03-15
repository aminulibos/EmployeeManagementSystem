namespace EmployeeManagement.Application.DTOs.Department;

public class DepartmentCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}