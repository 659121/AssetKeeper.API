using CoreLogic.Domain;

namespace CoreLogic.Interfaces;
public interface IUserRepository
{
    Task<List<User>> GetUsersWithRolesAsync(CancellationToken ct = default);
    Task<User?> GetUserWithRolesByIdAsync(int userId, CancellationToken ct = default);
    Task<User?> GetUserWithRolesByUsernameAsync(string username, CancellationToken ct = default);
    Task UpdateUserAsync(User user, CancellationToken ct = default);
    Task DeleteUserAsync(int userId, CancellationToken ct = default);
    Task<bool> UserExistsAsync(int userId, CancellationToken ct = default);
    Task<List<Role>> GetRolesListAsync(CancellationToken ct = default);
}