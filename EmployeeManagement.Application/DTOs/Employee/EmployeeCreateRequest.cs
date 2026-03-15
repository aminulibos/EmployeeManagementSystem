namespace EmployeeManagement.Application.DTOs.Employee;

public class EmployeeCreateRequest
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public int DepartmentId { get; set; }
    public int DesignationId { get; set; }
}

