using Microsoft.EntityFrameworkCore;
using Startawy.Domain.Entities;
using Startawy.Domain.Interfaces;
using Startawy.Infrastructure.Data;

namespace Startawy.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(string userId)
        => await _context.Users.FindAsync(userId);

    public async Task<User?> GetByEmailAsync(string email)
    {
        var normalized = email.ToLower().Trim();
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == normalized);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var normalized = email.ToLower().Trim();
        return await _context.Users
            .AnyAsync(u => u.Email.ToLower() == normalized);
    }

    public async Task<User> CreateAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
