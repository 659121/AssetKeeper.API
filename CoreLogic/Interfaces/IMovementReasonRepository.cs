using CoreLogic.Domain;

namespace CoreLogic.Interfaces;
public interface IMovementReasonRepository : IBaseRepository<MovementReason>
{
    Task<MovementReason> GetByCodeAsync(string code);
}