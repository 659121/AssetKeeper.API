using CoreLogic.Domain;
using CoreLogic.Models;

namespace CoreLogic.Interfaces;

public interface IDeviceMovementRepository
{
    Task<List<DeviceMovementDto>> GetHistoryByDeviceAsync(Guid deviceId, CancellationToken ct = default);
    Task AddAsync(DeviceMovement movement, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<DeviceMovement> movements, CancellationToken ct = default);
}