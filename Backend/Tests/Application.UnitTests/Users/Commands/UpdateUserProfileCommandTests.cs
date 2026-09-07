using Application.Abstractions.Database;
using Application.Abstractions.Respositories;
using Application.UseCases.Users.Commands.UpdateUserProfile;
using Domain.Entities;
using Domain.Errors;
using NSubstitute;

namespace Application.UnitTests.Users.Commands;

public sealed class UpdateUserProfileCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateUserProfileCommandHandler _handler;

    public UpdateUserProfileCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new UpdateUserProfileCommandHandler(
            _userRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenUserNotFound()
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
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenUsernameAlreadyInUse()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var user = UserFixture.CreateUser(username: "notjohndoe");

        _userRepository
            .GetByIdAsync(user.Id, cancellationToken)
            .Returns(user);

        _userRepository
            .ExistsByUsernameAsync(Arg.Any<string>(), cancellationToken)
            .Returns(true);

        var command = CreateValidCommand(user.Id);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.UsernameAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenEmailAlreadyInUse()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var user = UserFixture.CreateUser(email: "notjohndoe@example.com");

        _userRepository
            .GetByIdAsync(user.Id, cancellationToken)
            .Returns(user);

        _userRepository
            .ExistsByEmailAsync(Arg.Any<string>(), cancellationToken)
            .Returns(true);

        var command = CreateValidCommand(user.Id);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.EmailAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenProfileUpdatedSuccessfully()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var user = UserFixture.CreateUser();

        _userRepository
            .GetByIdAsync(user.Id, cancellationToken)
            .Returns(user);

        _userRepository
            .ExistsByUsernameAsync(Arg.Any<string>(), cancellationToken)
            .Returns(false);

        _userRepository
            .ExistsByEmailAsync(Arg.Any<string>(), cancellationToken)
            .Returns(false);

        var command = CreateValidCommand(user.Id);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);

        _userRepository.Received(1).Update(user);
        await _unitOfWork.Received(1).SaveChangesAsync(cancellationToken);
    }

    private static UpdateUserProfileCommand CreateValidCommand(Guid userId)
    {
        return new UpdateUserProfileCommand(
            userId: userId,
            name: "John Doe",
            username: "johndoe",
            email: "johndoe@example.com",
            updatedBy: null);
    }
}
