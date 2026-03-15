namespace EmployeeManagement.Domain.Entities;

public class Salary
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime? DisbursedDate { get; set; }

    public Employee? Employee { get; set; }
}