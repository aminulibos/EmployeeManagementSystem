﻿﻿namespace EmployeeManagement.Domain.Entities;

public class Employee
{
    public int Id { get; set; }
    public bool IsActive { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime JoiningDate { get; set; }

    public ICollection<Salary>? Salaries { get; set; }
    public Department? Department { get; set; }
    public int DepartmentId { get; set; }
    public Designation? Designation { get; set; }
    public int DesignationId { get; set; }
}

