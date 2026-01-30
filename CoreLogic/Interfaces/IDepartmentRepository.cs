using CoreLogic.Domain;
using CoreLogic.Models;

namespace CoreLogic.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync(CancellationToken ct = default);
    Task<Department?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<DepartmentStatsDto>> GetStatisticsAsync(CancellationToken ct = default);
}