using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

internal class AuthRepository(AppContext context) : IAuthRepository
{
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        user.RegDate = DateTime.Now;
        await context.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync();
    }

    public async Task<UserWithRolesDto?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);

        if (user == null)
            return null;

        return new UserWithRolesDto
        {
            Id = user.Id,
            Username = user.Username,
            PasswordHash = user.PasswordHash,
            Salt = user.Salt,
            IsActive = user.IsActive,
            Roles = user.UserRoles?.Select(ur => ur.Role.Name).ToList() ?? new List<string>()
        };
    }
}