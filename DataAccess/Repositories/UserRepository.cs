using CoreLogic.Domain;
using CoreLogic.Interfaces;
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
        var entities = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .OrderByDescending(u => u.RegDate)
            .ToListAsync(ct);

        return entities.Select(MapToDomain).ToList();
    }

    public async Task<User?> GetUserWithRolesByIdAsync(int userId, CancellationToken ct = default)
    {
        var entity = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        return entity != null ? MapToDomain(entity) : null;
    }

    public async Task<User?> GetUserWithRolesByUsernameAsync(string username, CancellationToken ct = default)
    {
        var entity = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, ct);

        return entity != null ? MapToDomain(entity) : null;
    }
    
    public async Task<bool> UpdateUserRolesAsync(int userId, List<string> roleNames, CancellationToken ct = default)
    {
        // Проверяем существование пользователя
        if (!await _context.Users.AnyAsync(u => u.Id == userId, ct))
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
        var newUserRoles = roleIds.Select(roleId => new DataAccess.Entities.UserRole  // ← ПОЛНОЕ ИМЯ!
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
        var entity = await _context.Users.FindAsync([userId], ct);
        if (entity != null)
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> UserExistsAsync(int userId, CancellationToken ct = default)
    {
        return await _context.Users.AnyAsync(u => u.Id == userId, ct);
    }

    public async Task<List<Role>> GetRolesListAsync(CancellationToken ct = default)
    {
        var roleEntities = await _context.Roles
            .OrderBy(u => u.Id)
            .ToListAsync(ct);

        return roleEntities.Select(entity => new Role
        {
            Id = entity.Id,
            Name = entity.Name
        }).ToList();
    }

    private User MapToDomain(DataAccess.Entities.User entity) => new()
    {
        Id = entity.Id,
        Username = entity.Username,
        PasswordHash = entity.PasswordHash,
        Salt = entity.Salt,
        IsActive = entity.IsActive,
        RegDate = entity.RegDate,
        LastLogin = entity.LastLogin,
        UserRoles = entity.UserRoles.Select(ur => new UserRole
        {
            UserId = ur.UserId,
            RoleId = ur.RoleId,
            Role = new Role
            {
                Id = ur.Role.Id,
                Name = ur.Role.Name
            }
        }).ToList()
    };

    // Маппинг: Domain → Entity
    private DataAccess.Entities.User MapToEntity(User domain) => new()
    {
        Id = domain.Id,
        Username = domain.Username,
        PasswordHash = domain.PasswordHash,
        Salt = domain.Salt,
        IsActive = domain.IsActive,
        RegDate = domain.RegDate,
        LastLogin = domain.LastLogin,
        UserRoles = domain.UserRoles.Select(ur => new DataAccess.Entities.UserRole
        {
            UserId = ur.UserId,
            RoleId = ur.RoleId
        }).ToList()
    };
}