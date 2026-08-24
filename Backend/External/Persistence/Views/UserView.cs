using Application.Abstractions.Database;
using Application.Abstractions.Views;
using Application.Dtos;
using Application.Dtos.Base;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Views;

internal sealed class UserView : IUserView
{
    private readonly IApplicationDbContext _context;

    public UserView(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Where(u => u.Id == id && !u.DeletedAt.HasValue)
            .Select(u =>
                new UserDto(
                u.Id,
                u.Name,
                u.Username,
                u.Email))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<PagedList<UserDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Users
            .Where(u => !u.DeletedAt.HasValue)
            .Select(u =>
                new UserDto(
                u.Id,
                u.Name,
                u.Username,
                u.Email));

        return await PagedList<UserDto>.CreateAsync(query, page, pageSize, cancellationToken);
    }
}
