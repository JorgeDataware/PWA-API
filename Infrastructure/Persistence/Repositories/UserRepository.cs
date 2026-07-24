using Microsoft.EntityFrameworkCore;
using PWA_API.Application.Interfaces.Repositories;
using PWA_API.Domain.Entities;

namespace PWA_API.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public Task<User?> GetByIdAsync(int id) =>
        context.Users.FindAsync(id).AsTask();

    public Task<User?> GetByEmailAsync(string email) =>
        context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public Task<User?> GetByUsernameAsync(string username) =>
        context.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task AddAsync(User user)
    {
        context.Users.Add(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public Task<int> CountActiveAdminsAsync() =>
        context.Users.CountAsync(u => u.IsActive && u.Role == Domain.Enums.UserRole.Admin);

    public Task<bool> ExistsByEmailAsync(string email) =>
        context.Users.AnyAsync(u => u.Email == email);

    public Task<bool> ExistsByUsernameAsync(string username) =>
        context.Users.AnyAsync(u => u.Username == username);
}
