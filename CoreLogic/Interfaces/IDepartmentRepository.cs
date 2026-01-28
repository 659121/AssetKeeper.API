namespace CoreLogic.Interfaces;
public interface IDepartmentRepository : IBaseRepository<Department>
{
    Task<List<DepartmentStatsDto>> GetStatisticsAsync();
}