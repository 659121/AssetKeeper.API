using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IDepartmentRepository : IBaseRepository<Department>
{
    Task<List<DepartmentStatsDto>> GetStatisticsAsync();
}