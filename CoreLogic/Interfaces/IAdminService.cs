using CoreLogic.Models.Admin;

namespace CoreLogic.Interfaces;
public interface IAdminService
{
    Task<List<UserDetails>> GetUsersAsync(CancellationToken ct);
    Task<UserDetails?> GetUserDetailsAsync(int userId, CancellationToken ct);
    Task<bool> UpdateUserAsync(int userId, UpdateUserCommand command, CancellationToken ct);
    Task<bool> DeleteUserAsync(int userId, CancellationToken ct);
    Task<List<string>> GetRolesAsync(CancellationToken ct);
}