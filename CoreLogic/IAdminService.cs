using CoreLogic.Models.DTO.Admin;

namespace CoreLogic;
public interface IAdminService
{
    Task<List<UserListItemDto>> GetUsersAsync(CancellationToken ct = default);
    Task<UserDetailsDto?> GetUserDetailsAsync(int userId, CancellationToken ct = default);
    Task<bool> UpdateUserAsync(int userId, UpdateUserRequest request, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(int userId, CancellationToken ct = default);
    Task<List<string>> GetRolesAsync(CancellationToken ct = default);
}