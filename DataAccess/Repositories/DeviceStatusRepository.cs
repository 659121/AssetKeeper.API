using CoreLogic.Domain;
using CoreLogic.Interfaces;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class DeviceStatusRepository : IDeviceStatusRepository
{
    private readonly InventoryDbContext _context;

    public DeviceStatusRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeviceStatus>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.DeviceStatuses
            .Where(s => s.IsActive)
            .OrderBy(s => s.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<DeviceStatus?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.DeviceStatuses
            .FirstOrDefaultAsync(s => s.Code == code && s.IsActive, ct);
    }
}