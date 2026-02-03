using CoreLogic.Domain;
using CoreLogic.Interfaces;
using CoreLogic.Models;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class DepartmentRepository : IDepartmentRepository
{
    private readonly InventoryDbContext _context;

    public DepartmentRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Department department, CancellationToken ct = default)
    {
        await _context.Departments.AddAsync(department, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<List<Department>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Departments
            .Where(d => d.IsActive)
            .OrderBy(d => d.Code)
            .ToListAsync(ct);
    }

    public async Task<Department?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Departments
            .FirstOrDefaultAsync(d => d.Id == id && d.IsActive, ct);
    }

    public async Task<List<DepartmentStatsDto>> GetStatisticsAsync(CancellationToken ct = default)
    {
        return await _context.Departments
            .Where(d => d.IsActive)
            .Select(d => new DepartmentStatsDto
            {
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                DeviceCount = _context.Devices
                    .Count(dev => dev.IsActive && dev.CurrentDepartmentId == d.Id),
                ActiveCount = _context.Devices
                    .Count(dev => dev.IsActive && 
                                 dev.CurrentDepartmentId == d.Id && 
                                 dev.CurrentStatusId == 1), // active status ID
                RepairCount = _context.Devices
                    .Count(dev => dev.IsActive && 
                                 dev.CurrentDepartmentId == d.Id && 
                                 dev.CurrentStatusId == 2) // repair status ID
            })
            .ToListAsync(ct);
    }
}