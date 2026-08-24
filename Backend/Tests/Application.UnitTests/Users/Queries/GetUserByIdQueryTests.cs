using Application.Abstractions.Views;
using Application.Dtos;
using Application.UseCases.Users.Queries.GetUserById;
using Domain.Errors;
using NSubstitute;

namespace Application.UnitTests.Users.Queries;

public sealed class GetUserByIdQueryHandlerTests
{
    private readonly IUserView _userView;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _userView = Substitute.For<IUserView>();

        _handler = new GetUserByIdQueryHandler(_userView);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenUserNotFound()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        // Arrange
        _userView
            .GetByIdAsync(Arg.Any<Guid>(), cancellationToken)
            .Returns(Task.FromResult<UserDto?>(null));

        var query = CreateValidQuery(Guid.NewGuid());

        // Act
        var result = await _handler.HandleAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenUserFound()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        // Arrange
        var userId = Guid.NewGuid();
        var userDto = UserFixture.CreateUserDto(userId);

        _userView
            .GetByIdAsync(userId, cancellationToken)
            .Returns(userDto);

        var query = CreateValidQuery(userId);

        // Act
        var result = await _handler.HandleAsync(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(userDto, result.Value);
    }

    private static GetUserByIdQuery CreateValidQuery(Guid userId)
    {
        return new GetUserByIdQuery(userId);
    }
}
