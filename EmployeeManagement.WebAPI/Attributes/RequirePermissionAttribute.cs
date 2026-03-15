namespace EmployeeManagement.WebAPI.Attributes;

/// <summary>
/// Permission-Based Authorization Attribute
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RequirePermissionAttribute(string permission) : Attribute
{
    public string Permission { get; } = permission;
}