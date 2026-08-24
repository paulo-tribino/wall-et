using Application.Abstractions.Database;
using Application.Abstractions.Respositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IApplicationDbContext _context;

    public UserRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Id == id && !u.DeletedAt.HasValue,
                cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(u =>
                u.Email == email && !u.DeletedAt.HasValue,
                cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(u =>
                u.Username == username && !u.DeletedAt.HasValue,
                cancellationToken);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }
}
