namespace EmployeeManagement.Application.DTOs.Designation;

public class DesignationCreateRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}