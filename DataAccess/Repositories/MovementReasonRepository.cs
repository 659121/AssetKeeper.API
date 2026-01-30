using CoreLogic.Domain;
using CoreLogic.Interfaces;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class MovementReasonRepository : IMovementReasonRepository
{
    private readonly InventoryDbContext _context;

    public MovementReasonRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovementReason>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.MovementReasons
            .Where(r => r.IsActive)
            .OrderBy(r => r.SortOrder)
            .ToListAsync(ct);
    }

    public async Task<MovementReason?> GetByCodeAsync(string code, CancellationToken ct = default)
    {
        return await _context.MovementReasons
            .FirstOrDefaultAsync(r => r.Code == code && r.IsActive, ct);
    }

    public async Task<MovementReason?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.MovementReasons
            .FirstOrDefaultAsync(r => r.Id == id && r.IsActive, ct);
    }
}