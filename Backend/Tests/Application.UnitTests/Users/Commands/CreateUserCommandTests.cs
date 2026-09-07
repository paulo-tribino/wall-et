using Application.Abstractions.Database;
using Application.Abstractions.Hashers;
using Application.Abstractions.Respositories;
using Application.UseCases.Users.Commands.CreateUser;
using Domain.Entities;
using Domain.Errors;
using NSubstitute;

namespace Application.UnitTests.Users.Commands;

public sealed class CreateUserCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new CreateUserCommandHandler(
            _userRepository,
            _passwordHasher,
            _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenUsernameAlreadyExists()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _userRepository
            .ExistsByUsernameAsync(Arg.Any<string>(), cancellationToken)
            .Returns(true);

        var command = CreateValidCommand();

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.UsernameAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsError_WhenEmailAlreadyExists()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _userRepository
            .ExistsByUsernameAsync(Arg.Any<string>(), cancellationToken)
            .Returns(false);

        _userRepository
            .ExistsByEmailAsync(Arg.Any<string>(), cancellationToken)
            .Returns(true);

        var command = CreateValidCommand();

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.EmailAlreadyInUse, result.Error);
    }

    [Fact]
    public async Task HandleAsync_ReturnsSuccess_WhenUserIsCreatedSuccessfully()
    {
        // Arrange
        var cancellationToken = TestContext.Current.CancellationToken;

        _userRepository
            .ExistsByUsernameAsync(Arg.Any<string>(), cancellationToken)
            .Returns(false);

        _userRepository
            .ExistsByEmailAsync(Arg.Any<string>(), cancellationToken)
            .Returns(false);

        _passwordHasher
            .Hash(Arg.Any<string>())
            .Returns("hashed_password");

        var command = CreateValidCommand();

        // Act
        var result = await _handler.HandleAsync(command, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);

        await _userRepository.Received(1).AddAsync(Arg.Any<User>(), cancellationToken);
        await _unitOfWork.Received(1).SaveChangesAsync(cancellationToken);
    }

    private static CreateUserCommand CreateValidCommand()
    {
        return new CreateUserCommand(
            name: "John Doe",
            username: "johndoe",
            email: "johndoe@example.com",
            password: "Password123",
            createdBy: null);
    }
}
