using Application.Abstractions.Messaging;
using Application.Abstractions.Views;
using Application.Dtos;
using Application.Dtos.Base;
using SharedKernel;

namespace Application.UseCases.Users.Queries.GetUsers;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, PagedList<UserDto>>
{
    private readonly IUserView _userView;

    public GetUsersQueryHandler(IUserView userView)
    {
        _userView = userView;
    }

    public async Task<Result<PagedList<UserDto>>> HandleAsync(
        GetUsersQuery query,
        CancellationToken cancellationToken)
    {
        var users = await _userView.GetPagedAsync(
            query.page,
            query.pageSize,
            cancellationToken);

        return Result.Success(users);
    }
}
