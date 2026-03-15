using EmployeeManagement.Application.DTOs.Designation;
using EmployeeManagement.Application.Interfaces;
using EmployeeManagement.Domain.Entities;
using EmployeeManagement.Infrastructure.Data.Repositories;
using EmployeeManagement.Infrastructure.Logging;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Application.Services;

public class DesignationService : IDesignationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuditLogger _auditLogger;
    private readonly ILogger<DesignationService> _logger;

    public DesignationService(IUnitOfWork unitOfWork, IAuditLogger auditLogger, ILogger<DesignationService> logger)
    {
        _unitOfWork = unitOfWork;
        _auditLogger = auditLogger;
        _logger = logger;
    }

    public async Task<DesignationResponse> CreateDesignationAsync(DesignationCreateRequest request)
    {
        try
        {
            var designation = new Designation
            {
                Title = request.Title,
                Description = request.Description
            };

            await _unitOfWork.Designations.AddAsync(designation).ConfigureAwait(false);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await _auditLogger.LogAsync("system", "CREATE", "Designation", $"Created designation: {designation.Title}", true).ConfigureAwait(false);
            _logger.LogInformation($"Designation created successfully: {designation.Title}");
            return new DesignationResponse
            {
                Id = designation.Id,
                Title = designation.Title,
                Description = designation.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating designation");
            await _auditLogger.LogAsync("system", "CREATE", "Designation", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<IEnumerable<DesignationResponse>> GetAllDesignationsAsync()
    {
        try
        {
            var designations = await _unitOfWork.Designations.GetAllAsync().ConfigureAwait(false);
            return designations.Select(d => new DesignationResponse
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving designations");
            throw;
        }
    }

    public async Task<DesignationResponse?> GetDesignationByIdAsync(int id)
    {
        try
        {
            var designation = await _unitOfWork.Designations.GetByIdAsync(id).ConfigureAwait(false);
            if (designation == null)
                return null;

            return new DesignationResponse
            {
                Id = designation.Id,
                Title = designation.Title,
                Description = designation.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving designation");
            throw;
        }
    }

    public async Task<DesignationResponse?> UpdateDesignationAsync(int id, DesignationCreateRequest request)
    {
        try
        {
            var designation = await _unitOfWork.Designations.GetByIdAsync(id).ConfigureAwait(false);
            if (designation == null)
                return null;

            designation.Title = request.Title;
            designation.Description = request.Description;

            _unitOfWork.Designations.Update(designation);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await _auditLogger.LogAsync("system", "UPDATE", "Designation", $"Updated designation: {designation.Title}", true).ConfigureAwait(false);
            _logger.LogInformation($"Designation updated successfully: {designation.Title}");
            return new DesignationResponse
            {
                Id = designation.Id,
                Title = designation.Title,
                Description = designation.Description
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating designation");
            await _auditLogger.LogAsync("system", "UPDATE", "Designation", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }

    public async Task<bool> DeleteDesignationAsync(int id)
    {
        try
        {
            var designation = await _unitOfWork.Designations.GetByIdAsync(id).ConfigureAwait(false);
            if (designation == null)
                return false;

            _unitOfWork.Designations.Remove(designation);
            await _unitOfWork.SaveChangesAsync().ConfigureAwait(false);
            await _auditLogger.LogAsync("system", "DELETE", "Designation", $"Deleted designation: {designation.Title}", true).ConfigureAwait(false);
            _logger.LogInformation($"Designation deleted successfully: {designation.Title}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting designation");
            await _auditLogger.LogAsync("system", "DELETE", "Designation", $"Error: {ex.Message}", false).ConfigureAwait(false);
            throw;
        }
    }
}