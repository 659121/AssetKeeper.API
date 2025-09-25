using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class UserRepository(AppContext context) : IUserRepository
    {
        public async Task<List<User>> GetUsersWithRolesAsync(CancellationToken ct = default)
        {
            return await context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .OrderByDescending(u => u.RegDate)
                .ToListAsync(ct);
        }

        public async Task<User?> GetUserWithRolesByIdAsync(int userId, CancellationToken ct = default)
        {
            return await context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);
        }

        public async Task<User?> GetUserWithRolesByUsernameAsync(string username, CancellationToken ct = default)
        {
            return await context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username == username, ct);
        }

        public async Task UpdateUserAsync(User user, CancellationToken ct = default)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync(ct);
        }

        public async Task DeleteUserAsync(int userId, CancellationToken ct = default)
        {
            var user = await context.Users.FindAsync(new object[] { userId }, ct);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync(ct);
            }
        }

        public async Task<bool> UserExistsAsync(int userId, CancellationToken ct = default)
        {
            return await context.Users.AnyAsync(u => u.Id == userId, ct);
        }

        public async Task<List<Role>> GetRolesListAsync(CancellationToken ct = default)
        {
            return await context.Roles
                .OrderBy(u => u.Id)
                .ToListAsync(ct);
        }
    }
}