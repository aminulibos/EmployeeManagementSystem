using EmployeeManagement.Application.DTOs.Designation;

namespace EmployeeManagement.Application.Interfaces;

public interface IDesignationService
{
    Task<DesignationResponse> CreateDesignationAsync(DesignationCreateRequest request);
    Task<IEnumerable<DesignationResponse>> GetAllDesignationsAsync();
    Task<DesignationResponse?> GetDesignationByIdAsync(int id);
    Task<DesignationResponse?> UpdateDesignationAsync(int id, DesignationCreateRequest request);
    Task<bool> DeleteDesignationAsync(int id);
}