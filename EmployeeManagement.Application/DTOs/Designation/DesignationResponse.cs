namespace EmployeeManagement.Application.DTOs.Designation;

public class DesignationResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}