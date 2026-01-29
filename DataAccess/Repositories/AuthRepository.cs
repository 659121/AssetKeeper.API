using CoreLogic.Domain;
using CoreLogic.Interfaces;
using DataAccess.DatabaseContexts;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;
internal class AuthRepository : IAuthRepository
{
    private readonly AuthContext _context;

    public AuthRepository(AuthContext context)
    {
        _context = context;
    }
    public async Task CreateUserAsync(CoreLogic.Domain.User user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);
    }
    
    public async Task<CoreLogic.Domain.User?> GetUserByUsernameAsync(string username, CancellationToken ct)
    {
        User? user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Username == username, ct);
        
        return user ?? null;
    }

    public async Task UpdateUserLastLoginAsync(int userId, DateTime loginTime, CancellationToken ct)
    {
        await _context.Users
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(u => u.LastLogin, loginTime),
            ct);
    }
}