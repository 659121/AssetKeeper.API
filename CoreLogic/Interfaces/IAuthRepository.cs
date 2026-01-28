using CoreLogic.Domain;

namespace CoreLogic.Interfaces;
public interface IAuthRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task UpdateUserLastLoginAsync(int userId, DateTime loginTime, CancellationToken cancellationToken = default);
}