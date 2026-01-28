using CoreLogic.Domain;
using CoreLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;
internal class AuthRepository : IAuthRepository
{
    private readonly AuthContext _context;

    public AuthRepository(AuthContext context)
    {
        _context = context;
    }
    public async Task CreateUserAsync(CoreLogic.Domain.User user, CancellationToken cancellationToken = default)
    {
        // Маппинг: доменная сущность → сущность БД
        DataAccess.Entities.User entity = MapToEntity(user);
        await _context.Users.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    private DataAccess.Entities.User MapToEntity(CoreLogic.Domain.User domainUser)
    {
        return new DataAccess.Entities.User
        {
            Id = domainUser.Id,
            Username = domainUser.Username,
            PasswordHash = domainUser.PasswordHash,
            Salt = domainUser.Salt,
            IsActive = domainUser.IsActive,
            RegDate = domainUser.RegDate,
            LastLogin = domainUser.LastLogin,
            UserRoles = domainUser.UserRoles.Select(ur => new DataAccess.Entities.UserRole
            {
                UserId = ur.UserId,
                RoleId = ur.RoleId
            }).ToList()
        };
    }
    
    public async Task<CoreLogic.Domain.User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var entity = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        
        return entity != null ? MapToDomain(entity) : null;
    }

    private CoreLogic.Domain.User MapToDomain(DataAccess.Entities.User entity)
    {
        return new CoreLogic.Domain.User
        {
            Id = entity.Id,
            Username = entity.Username,
            PasswordHash = entity.PasswordHash,
            Salt = entity.Salt,
            IsActive = entity.IsActive,
            RegDate = entity.RegDate,
            LastLogin = entity.LastLogin,
            UserRoles = entity.UserRoles.Select(ur => new CoreLogic.Domain.UserRole
            {
                UserId = ur.UserId,
                RoleId = ur.RoleId,
                Role = new CoreLogic.Domain.Role
                {
                    Id = ur.Role.Id,
                    Name = ur.Role.Name
                }
            }).ToList()
        };
    }

    public async Task UpdateUserLastLoginAsync(int userId, DateTime loginTime, CancellationToken cancellationToken = default)
    {
        await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.LastLogin, loginTime),
            cancellationToken);
    }
}