using Application.Abstractions.Messaging;
using Application.Abstractions.Views;
using Application.Dtos;
using Domain.Errors;
using SharedKernel;

namespace Application.UseCases.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserView _userView;

    public GetUserByIdQueryHandler(IUserView userView)
    {
        _userView = userView;
    }

    public async Task<Result<UserDto>> HandleAsync(
        GetUserByIdQuery query,
        CancellationToken cancellationToken)
    {
        var user = await _userView.GetByIdAsync(query.userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<UserDto>(UserErrors.NotFound);
        }

        return Result.Success(user);
    }
}
