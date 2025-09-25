using DataAccess.Models;

namespace DataAccess.Interfaces;
public interface IAuthRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task UpdateUserLastLoginAsync(int userId, DateTime loginTime, CancellationToken cancellationToken = default);
}