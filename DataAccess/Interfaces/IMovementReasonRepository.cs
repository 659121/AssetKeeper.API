using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IMovementReasonRepository : IBaseRepository<MovementReason>
{
    Task<MovementReason> GetByCodeAsync(string code);
}