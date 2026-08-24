using Application.Dtos;
using Application.Dtos.Base;

namespace Application.Abstractions.Views;

public interface IUserView
{
    Task<UserDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedList<UserDto>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken);
}
