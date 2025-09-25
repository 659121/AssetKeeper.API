using CoreLogic.Models.DTO.Admin;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace CoreLogic;

internal class AdminService(IUserRepository userRepository) : IAdminService
{
    public async Task<List<UserListItemDto>> GetUsersAsync(CancellationToken ct = default)
    {
        var users = await userRepository.GetUsersWithRolesAsync(ct);

        return users.Select(u => new UserListItemDto(
            u.Id,
            u.Username,
            u.RegDate,
            u.LastLogin,
            u.IsActive,
            u.UserRoles.Select(ur => ur.Role.Name).ToList()
        )).ToList();
    }

    public async Task<UserDetailsDto?> GetUserDetailsAsync(int userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetUserWithRolesByIdAsync(userId, ct);
        if (user == null) return null;

        return new UserDetailsDto(
            user.Id,
            user.Username,
            user.RegDate,
            user.LastLogin,
            user.IsActive,
            user.UserRoles.Select(ur => ur.Role.Name).ToList()
        );
    }

    public async Task<bool> UpdateUserAsync(int userId, UpdateUserRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetUserWithRolesByIdAsync(userId, ct);
        if (user == null) return false;

        // Обновление активности
        if (request.IsActive.HasValue)
            user.IsActive = request.IsActive.Value;

        // Обновление ролей
        if (request.Roles != null)
        {
            // Удаляем текущие роли
            user.UserRoles.Clear();

            // Добавляем новые роли
            var rolesToAdd = await userRepository.GetRolesListAsync(ct);

            foreach (var role in rolesToAdd)
            {
                user.UserRoles.Add(new UserRole { UserId = userId, RoleId = role.Id });
            }
        }

        await userRepository.UpdateUserAsync(user, ct);
        return true;
    }

    public async Task<bool> DeleteUserAsync(int userId, CancellationToken ct = default)
    {
        if (!await userRepository.UserExistsAsync(userId, ct))
            return false;

        await userRepository.DeleteUserAsync(userId, ct);
        return true;
    }
}