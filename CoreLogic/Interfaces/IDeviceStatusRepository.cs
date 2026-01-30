using CoreLogic.Domain;

namespace CoreLogic.Interfaces;

public interface IDeviceStatusRepository
{
    Task<List<DeviceStatus>> GetAllAsync(CancellationToken ct = default);
    Task<DeviceStatus?> GetByCodeAsync(string code, CancellationToken ct = default);
}