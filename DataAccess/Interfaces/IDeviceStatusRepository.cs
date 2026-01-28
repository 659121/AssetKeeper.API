using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IDeviceStatusRepository : IBaseRepository<DeviceStatus>
{
    Task<DeviceStatus> GetByCodeAsync(string code);
}