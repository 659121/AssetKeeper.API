using CoreLogic.Domain;
using CoreLogic.Interfaces;
using CoreLogic.Models;

namespace CoreLogic.Services;

internal class InventoryService : IInventoryService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IDeviceStatusRepository _statusRepository;
    private readonly IMovementReasonRepository _reasonRepository;
    private readonly IDeviceMovementRepository _movementRepository;

    public InventoryService(
        IDeviceRepository deviceRepository,
        IDepartmentRepository departmentRepository,
        IDeviceStatusRepository statusRepository,
        IMovementReasonRepository reasonRepository,
        IDeviceMovementRepository movementRepository)
    {
        _deviceRepository = deviceRepository;
        _departmentRepository = departmentRepository;
        _statusRepository = statusRepository;
        _reasonRepository = reasonRepository;
        _movementRepository = movementRepository;
    }

    public async Task<PagedResult<Device>> GetDevicesAsync(DeviceFilter filter, CancellationToken ct = default)
    {
        return await _deviceRepository.GetFilteredAsync(filter, ct);
    }

    public async Task<Device?> GetDeviceByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _deviceRepository.GetWithDetailsAsync(id, ct);
    }

    public async Task<List<DeviceMovementDto>> GetDeviceHistoryAsync(Guid deviceId, CancellationToken ct = default)
    {
        return await _movementRepository.GetHistoryByDeviceAsync(deviceId, ct);
    }

    public async Task<List<DepartmentStatsDto>> GetDepartmentStatisticsAsync(CancellationToken ct = default)
    {
        return await _departmentRepository.GetStatisticsAsync(ct);
    }

    public async Task<List<Department>> GetDepartmentsAsync(CancellationToken ct = default)
    {
        return await _departmentRepository.GetAllAsync(ct);
    }

    public async Task<List<DeviceStatus>> GetDeviceStatusesAsync(CancellationToken ct = default)
    {
        return await _statusRepository.GetAllAsync(ct);
    }

    public async Task<List<MovementReason>> GetMovementReasonsAsync(CancellationToken ct = default)
    {
        return await _reasonRepository.GetAllAsync(ct);
    }

    public async Task<Guid> CreateDeviceAsync(Device device, CancellationToken ct = default)
    {
        device.Id = Guid.NewGuid();
        device.CreatedAt = DateTime.UtcNow;
        device.UpdatedAt = DateTime.UtcNow;
        
        // Устанавливаем статус по умолчанию, если не указан
        if (device.CurrentStatusId == 0)
        {
            var activeStatus = await _statusRepository.GetByCodeAsync("active", ct);
            device.CurrentStatusId = activeStatus?.Id ?? 1;
        }

        await _deviceRepository.AddAsync(device, ct);
        await _deviceRepository.SaveChangesAsync(ct);
        return device.Id;
    }

    public async Task<bool> UpdateDeviceAsync(Device device, CancellationToken ct = default)
    {
        var existing = await _deviceRepository.GetByIdAsync(device.Id, ct);
        if (existing == null) return false;

        device.UpdatedAt = DateTime.UtcNow;
        await _deviceRepository.UpdateAsync(device, ct);
        await _deviceRepository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteDeviceAsync(Guid id, CancellationToken ct = default)
    {
        var existing = await _deviceRepository.GetByIdAsync(id, ct);
        if (existing == null) return false;

        await _deviceRepository.DeleteAsync(id, ct);
        await _deviceRepository.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> MoveDeviceAsync(Guid deviceId, Guid toDepartmentId, Guid reasonId, string movedBy, string? note = null, CancellationToken ct = default)
    {
        var device = await _deviceRepository.GetByIdAsync(deviceId, ct);
        if (device == null) return false;

        var reason = await _reasonRepository.GetByIdAsync(reasonId, ct);
        if (reason == null) return false;

        // Создаем запись в истории
        var movement = new DeviceMovement
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            FromDepartmentId = device.CurrentDepartmentId,
            ToDepartmentId = toDepartmentId,
            ReasonId = reasonId,
            MovedAt = DateTime.UtcNow,
            MovedBy = movedBy,
            Note = note
        };

        await _movementRepository.AddAsync(movement, ct);

        // Обновляем текущее состояние устройства
        device.CurrentDepartmentId = toDepartmentId;
        device.UpdatedAt = DateTime.UtcNow;

        // Автоматическое обновление статуса при определенных причинах
        if (reason.Code == "repair")
        {
            var repairStatus = await _statusRepository.GetByCodeAsync("repair", ct);
            device.CurrentStatusId = repairStatus?.Id ?? device.CurrentStatusId;
        }
        else if (reason.Code == "disposed")
        {
            var disposedStatus = await _statusRepository.GetByCodeAsync("disposed", ct);
            device.CurrentStatusId = disposedStatus?.Id ?? device.CurrentStatusId;
        }

        await _deviceRepository.UpdateAsync(device, ct);
        await _deviceRepository.SaveChangesAsync(ct);
        return true;
    }
}