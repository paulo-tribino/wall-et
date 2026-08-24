using Application.Abstractions.Views;
using Application.Dtos;
using Application.Dtos.Base;
using Application.UseCases.Users.Queries.GetUsers;
using NSubstitute;

namespace Application.UnitTests.Users.Queries;

public sealed class GetUsersQueryHandlerTests
{
    private readonly IUserView _userView;
    private readonly GetUsersQueryHandler _handler;

    public GetUsersQueryHandlerTests()
    {
        _userView = Substitute.For<IUserView>();

        _handler = new GetUsersQueryHandler(_userView);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WithPagedList()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        _userView
            .GetPagedAsync(Arg.Any<int>(), Arg.Any<int>(), cancellationToken)
            .Returns(Task.FromResult<PagedList<UserDto>>(null!));

        var query = new GetUsersQuery(page: 1, pageSize: 10);

        // Act
        var result = await _handler.HandleAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
