using CoreLogic.Domain;
using CoreLogic.Interfaces;
using CoreLogic.Models.Admin;
using System.Data;

namespace CoreLogic.Services;
internal class AdminService : IAdminService
{
    private readonly IUserRepository _userRepository;

    public AdminService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDetails>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await _userRepository.GetUsersWithRolesAsync(ct);

        return users.Select(MapToUserDetails).ToList();
    }

    public async Task<UserDetails?> GetUserDetailsAsync(int userId, CancellationToken ct = default)
    {
        var user = await _userRepository.GetUserWithRolesByIdAsync(userId, ct);
        return user != null ? MapToUserDetails(user) : null;
    }

    public async Task<bool> UpdateUserAsync(int userId, UpdateUserCommand command, CancellationToken ct = default)
    {
        var user = await _userRepository.GetUserWithRolesByIdAsync(userId, ct);
        if (user == null) return false;

        bool modified = false;

        // Обновление активности
        if (command.IsActive.HasValue && user.IsActive != command.IsActive.Value)
        {
            user.IsActive = command.IsActive.Value;
            modified = true;
        }

        // Обновление ролей
        if (command.Roles != null)
        {
            if (await _userRepository.UpdateUserRolesAsync(userId, command.Roles, ct))
                modified = true;
        }

        // Сохраняем изменения основного пользователя (если были)
        if (modified)
            await _userRepository.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId, CancellationToken ct = default)
    {
        if (!await _userRepository.UserExistsAsync(userId, ct))
            return false;

        await _userRepository.DeleteUserAsync(userId, ct);
        return true;
    }

    public async Task<List<string>> GetRolesAsync(CancellationToken ct = default)
    {
        var roles = await _userRepository.GetRolesListAsync(ct);
        return roles.Select(r => r.Name).ToList();
    }

    private static UserDetails MapToUserDetails(User user) => new(
        user.Id,
        user.Username,
        user.RegDate,
        user.LastLogin,
        user.IsActive,
        user.UserRoles.Select(ur => ur.Role.Name).ToList()
    );
}