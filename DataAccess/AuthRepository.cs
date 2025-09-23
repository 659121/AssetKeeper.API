using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

internal class AuthRepository(AppContext context) : IAuthRepository
{
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        return user;
    }
}