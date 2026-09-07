using Application.Abstractions.Database;
using Application.Abstractions.Hashers;
using Application.Abstractions.Respositories;
using Application.UseCases.Users.Commands.UpdateUserPassword;
using Domain.Entities;
using Domain.Errors;
using NSubstitute;

namespace Application.UnitTests.Users.Commands;

public sealed class UpdateUserPasswordCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateUserPasswordCommandHandler _handler;

    public UpdateUserPasswordCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new UpdateUserPasswordCommandHandler(
            _userRepository,
            _passwordHasher,
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
    public async Task HandleAsync_ReturnsError_WhenCurrentPasswordIsInvalid()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var user = UserFixture.CreateUser();

        _userRepository
            .GetByIdAsync(user.Id, cancellationToken)
            .Returns(user);

        _passwordHasher
            .Verify(Arg.Any<string>(), user.PasswordHash)
            .Returns(false);

        var command = CreateValidCommand(user.Id);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.InvalidCurrentPassword, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenPasswordUpdatedSuccessfully()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        var user = UserFixture.CreateUser(passwordHash: "hashed_current_password");

        _userRepository
            .GetByIdAsync(user.Id, cancellationToken)
            .Returns(user);

        _passwordHasher
            .Verify(Arg.Any<string>(), user.PasswordHash)
            .Returns(true);

        _passwordHasher
            .Hash(Arg.Any<string>())
            .Returns("hashed_new_password");

        var command = CreateValidCommand(user.Id);

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("hashed_new_password", user.PasswordHash);

        _userRepository.Received(1).Update(user);
        await _unitOfWork.Received(1).SaveChangesAsync(cancellationToken);
    }

    private static UpdateUserPasswordCommand CreateValidCommand(Guid userId)
    {
        return new UpdateUserPasswordCommand(
            userId: userId,
            currentPassword: "CurrentPassword123",
            newPassword: "NewPassword456");
    }
}
