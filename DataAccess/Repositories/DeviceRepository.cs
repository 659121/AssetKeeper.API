using CoreLogic.Domain;
using CoreLogic.Interfaces;
using CoreLogic.Models;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class DeviceRepository : IDeviceRepository
{
    private readonly InventoryDbContext _context;

    public DeviceRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Devices.FindAsync(new object[] { id }, ct);
    }

    public async Task<Device?> GetWithDetailsAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Devices
            .Include(d => d.CurrentDepartment)
            .Include(d => d.CurrentStatus)
            .Include(d => d.Movements)
            .ThenInclude(m => m.ToDepartment)
            .Include(d => d.Movements)
            .ThenInclude(m => m.Reason)
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, ct);
    }

    public async Task<PagedResult<Device>> GetFilteredAsync(DeviceFilter filter, CancellationToken ct = default)
    {
        filter.Validate();
        var query = _context.Devices
            .Where(d => d.IsActive)
            .Include(d => d.CurrentDepartment)
            .Include(d => d.CurrentStatus)
            .AsQueryable();

        if (filter.DepartmentId.HasValue)
            query = query.Where(d => d.CurrentDepartmentId == filter.DepartmentId.Value);

        if (filter.StatusId.HasValue)
            query = query.Where(d => d.CurrentStatusId == filter.StatusId.Value);

        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            var searchText = filter.SearchText;
            query = query.Where(d =>
                EF.Functions.Like(d.Name, $"%{searchText}%") ||
                (d.InventoryNumber != null && EF.Functions.Like(d.InventoryNumber, $"%{searchText}%")) ||
                (d.Description != null && EF.Functions.Like(d.Description, $"%{searchText}%")));
        }

        query = filter.SortBy.ToLower() switch
        {
            "name" => filter.SortDescending 
                ? query.OrderByDescending(d => d.Name) 
                : query.OrderBy(d => d.Name),
            "inventory" => filter.SortDescending 
                ? query.OrderByDescending(d => d.InventoryNumber) 
                : query.OrderBy(d => d.InventoryNumber),
            "department" => filter.SortDescending 
                ? query.OrderByDescending(d => d.CurrentDepartmentId) 
                : query.OrderBy(d => d.CurrentDepartmentId),
            "status" => filter.SortDescending 
                ? query.OrderByDescending(d => d.CurrentStatusId) 
                : query.OrderBy(d => d.CurrentStatusId),
            _ => query.OrderBy(d => d.Name)
        };

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return new PagedResult<Device>
        {
            Items = items,
            TotalCount = total,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<int> GetCountByDepartmentAsync(Guid departmentId, CancellationToken ct = default)
    {
        return await _context.Devices
            .CountAsync(d => d.IsActive && d.CurrentDepartmentId == departmentId, ct);
    }

    public async Task<int> GetCountByStatusAsync(int statusId, CancellationToken ct = default)
    {
        return await _context.Devices
            .CountAsync(d => d.IsActive && d.CurrentStatusId == statusId, ct);
    }

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        await _context.Devices.AddAsync(device, ct);
    }

    public async Task UpdateAsync(Device device, CancellationToken ct = default)
    {
        _context.Devices.Update(device);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var device = await _context.Devices.FindAsync(new object[] { id }, ct);
        if (device != null)
        {
            device.IsActive = false;
        }
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}