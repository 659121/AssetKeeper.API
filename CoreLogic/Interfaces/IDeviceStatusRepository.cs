using CoreLogic.Domain;

namespace CoreLogic.Interfaces;
public interface IDeviceStatusRepository : IBaseRepository<DeviceStatus>
{
    Task<DeviceStatus> GetByCodeAsync(string code);
}