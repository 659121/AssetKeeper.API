using DataAccess.Models;

namespace DataAccess;
public interface IAuthRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default);
}