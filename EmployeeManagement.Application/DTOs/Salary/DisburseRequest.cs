namespace EmployeeManagement.Application.DTOs.Salary;

/// <summary>
/// DTO for salary disbursement request
/// </summary>
public class DisburseRequest
{
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}

