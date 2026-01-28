using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IDeviceMovementRepository : IBaseRepository<DeviceMovement>
{
    Task<List<DeviceMovementDto>> GetHistoryByDeviceAsync(int deviceId);
    Task AddRangeAsync(IEnumerable<DeviceMovement> movements);
}