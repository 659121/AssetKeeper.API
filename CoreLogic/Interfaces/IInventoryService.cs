using CoreLogic.Domain;
using CoreLogic.Models;

namespace CoreLogic.Services;

public interface IInventoryService
{
    Task<PagedResult<Device>> GetDevicesAsync(DeviceFilter filter, CancellationToken ct = default);
    Task<Device?> GetDeviceByIdAsync(Guid id, CancellationToken ct = default);
    Task<List<DeviceMovementDto>> GetDeviceHistoryAsync(Guid deviceId, CancellationToken ct = default);
    Task<List<DepartmentStatsDto>> GetDepartmentStatisticsAsync(CancellationToken ct = default);
    Task<List<Department>> GetDepartmentsAsync(CancellationToken ct = default);
    Task<List<DeviceStatus>> GetDeviceStatusesAsync(CancellationToken ct = default);
    Task<List<MovementReason>> GetMovementReasonsAsync(CancellationToken ct = default);
    
    Task<Guid> CreateDeviceAsync(Device device, string createdBy, CancellationToken ct = default);
    Task<Guid> CreateDepartmentAsync(Department department, CancellationToken ct = default);
    Task<bool> CreateReasonAsync(MovementReason reason, CancellationToken ct = default);
    Task<bool> UpdateDeviceAsync(Device device, CancellationToken ct = default);
    Task<bool> DeleteDeviceAsync(Guid id, CancellationToken ct = default);
    Task<bool> MoveDeviceAsync(Guid deviceId, Guid toDepartmentId, Guid reasonId, string movedBy, string? newSticker = null, string? note = null, string? representative = null, CancellationToken ct = default);
}