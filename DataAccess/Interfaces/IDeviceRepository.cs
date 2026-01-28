using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IDeviceRepository : IBaseRepository<Device>
{
    Task<Device> GetWithDetailsAsync(int id);
    Task<PagedResult<DeviceDto>> GetFilteredAsync(DeviceFilter filter);
    Task<int> GetCountByDepartmentAsync(int departmentId);
    Task<int> GetCountByStatusAsync(int statusId);
}