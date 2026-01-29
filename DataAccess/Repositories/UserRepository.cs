using CoreLogic.Domain;
using CoreLogic.Interfaces;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly AuthContext _context;

    public UserRepository(AuthContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetUsersWithRolesAsync(CancellationToken ct = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderByDescending(u => u.RegDate)
            .ToListAsync(ct);
    }

    public async Task<User?> GetUserWithRolesByIdAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
    }

    public async Task<User?> GetUserWithRolesByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, ct);
    }
    
    public async Task<bool> UpdateUserRolesAsync(int userId, List<string> roleNames, CancellationToken ct = default)
    {
        // Проверяем существование пользователя
        if (!await UserExistsAsync(userId, ct))
            return false;

        // Получаем ID ролей по именам
        var roleIds = await _context.Roles
            .Where(r => roleNames.Contains(r.Name))
            .Select(r => r.Id)
            .ToListAsync(ct);

        // Удаляем все существующие роли пользователя
        var existingRoles = await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .ToListAsync(ct);
        
        _context.UserRoles.RemoveRange(existingRoles);

        // Добавляем новые роли
        var newUserRoles = roleIds.Select(roleId => new UserRole  // ← ПОЛНОЕ ИМЯ!
        {
            UserId = userId,
            RoleId = roleId
        });

        _context.UserRoles.AddRange(newUserRoles);
        await _context.SaveChangesAsync(ct);
        
        return true;
    }
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        Console.WriteLine("\ndspjd\n");
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteUserAsync(int userId, CancellationToken ct = default)
    {
        var user = await _context.Users.FindAsync(new object[] { userId }, ct);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> UserExistsAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId, ct);
    }

    public async Task<List<Role>> GetRolesListAsync(CancellationToken ct = default)
    {
        return await _context.Roles
            .OrderBy(r => r.Id)
            .ToListAsync(ct);
    }
}