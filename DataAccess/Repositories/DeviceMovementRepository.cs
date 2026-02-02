using CoreLogic.Domain;
using CoreLogic.Interfaces;
using CoreLogic.Models;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class DeviceMovementRepository : IDeviceMovementRepository
{
    private readonly InventoryDbContext _context;

    public DeviceMovementRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeviceMovementDto>> GetHistoryByDeviceAsync(Guid deviceId, CancellationToken ct = default)
    {
        var movements = await _context.DeviceMovements
            .Where(m => m.DeviceId == deviceId)
            .Include(m => m.FromDepartment)
            .Include(m => m.ToDepartment)
            .Include(m => m.Reason)
            .OrderByDescending(m => m.MovedAt)
            .ToListAsync(ct);

        // Добавьте отладочный вывод
        Console.WriteLine($"Found {movements.Count} movements for device {deviceId}");
        foreach (var m in movements)
        {
            Console.WriteLine($"Movement ID: {m.Id}, FromDeptId: {m.FromDepartmentId}, ToDeptId: {m.ToDepartmentId}");
            Console.WriteLine($"FromDept loaded: {m.FromDepartment != null}, ToDept loaded: {m.ToDepartment != null}");
        }
        
        return movements.Select(m => new DeviceMovementDto
        {
            Id = m.Id,
            MovedAt = m.MovedAt,
            FromDepartmentName = m.FromDepartment?.Name,
            ToDepartmentName = m.ToDepartment?.Name ?? string.Empty,
            ReasonName = m.Reason?.Name ?? string.Empty,
            MovedBy = m.MovedBy,
            Note = m.Note
        }).ToList();
    }

    public async Task AddAsync(DeviceMovement movement, CancellationToken ct = default)
    {
        await _context.DeviceMovements.AddAsync(movement, ct);
    }

    public async Task AddRangeAsync(IEnumerable<DeviceMovement> movements, CancellationToken ct = default)
    {
        await _context.DeviceMovements.AddRangeAsync(movements, ct);
    }
}