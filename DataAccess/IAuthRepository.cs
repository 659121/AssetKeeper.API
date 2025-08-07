using DataAccess.Models;

namespace DataAccess;
public interface IAuthRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
    Task<UserWithRolesDto?> GetUserByUsernameAsync(String username, CancellationToken cancellationToken = default);
}