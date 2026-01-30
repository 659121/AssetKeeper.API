using CoreLogic.Domain;

namespace CoreLogic.Interfaces;

public interface IMovementReasonRepository
{
    Task<List<MovementReason>> GetAllAsync(CancellationToken ct = default);
    Task<MovementReason?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<MovementReason?> GetByIdAsync(Guid id, CancellationToken ct = default);
}