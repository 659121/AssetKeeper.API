using CoreLogic.Domain;
using CoreLogic.Models;

namespace CoreLogic.Interfaces;

public interface IDeviceRepository
{
    Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Device?> GetWithDetailsAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<Device>> GetFilteredAsync(DeviceFilter filter, CancellationToken ct = default);
    Task<int> GetCountByDepartmentAsync(Guid departmentId, CancellationToken ct = default);
    Task<int> GetCountByStatusAsync(int statusId, CancellationToken ct = default);
    Task AddAsync(Device device, CancellationToken ct = default);
    Task UpdateAsync(Device device, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}