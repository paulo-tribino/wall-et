using Application.Abstractions.Database;
using Application.Abstractions.Hashers;
using Application.Abstractions.Messaging;
using Application.Abstractions.Respositories;
using Domain.Entities;
using Domain.Errors;
using SharedKernel;

namespace Application.UseCases.Users.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> HandleAsync(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.ExistsByUsernameAsync(command.username, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.UsernameAlreadyInUse);
        }

        if (await _userRepository.ExistsByEmailAsync(command.email, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailAlreadyInUse);
        }

        var passwordHash = _passwordHasher.Hash(command.password);

        var user = User.Create(
            command.name,
            command.username,
            command.email,
            passwordHash,
            command.createdBy);

        await _userRepository.AddAsync(user, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Id);
    }
}
