using Application.Abstractions.Database;
using Application.Abstractions.Respositories;
using Application.UseCases.Users.Commands.DeleteUser;
using Domain.Entities;
using NSubstitute;

namespace Application.UnitTests.Users.Commands;

public sealed class DeleteUserCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new DeleteUserCommandHandler(
            _userRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenUserNotFound()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _userRepository
            .GetByIdAsync(Arg.Any<Guid>(), cancellationToken)
            .Returns(Task.FromResult<User?>(null));

        var command = CreateValidCommand(Guid.NewGuid());

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenUserDeleted()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var user = UserFixture.CreateUser();

        _userRepository
            .GetByIdAsync(user.Id, cancellationToken)
            .Returns(user);

        var command = CreateValidCommand(user.Id);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);

        _userRepository.Received(1).Update(user);
        await _unitOfWork.Received(1).SaveChangesAsync(cancellationToken);
    }

    private static DeleteUserCommand CreateValidCommand(Guid userId)
    {
        return new DeleteUserCommand(
            userId,
            deletedBy: null);
    }
}
