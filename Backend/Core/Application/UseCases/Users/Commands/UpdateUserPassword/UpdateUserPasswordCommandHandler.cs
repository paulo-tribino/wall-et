using Application.Abstractions.Database;
using Application.Abstractions.Hashers;
using Application.Abstractions.Messaging;
using Application.Abstractions.Respositories;
using Domain.Errors;
using SharedKernel;

namespace Application.UseCases.Users.Commands.UpdateUserPassword;

internal sealed class UpdateUserPasswordCommandHandler : ICommandHandler<UpdateUserPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserPasswordCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(
        UpdateUserPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (!_passwordHasher.Verify(command.currentPassword, user.PasswordHash))
        {
            return Result.Failure(UserErrors.InvalidCurrentPassword);
        }

        var newPasswordHash = _passwordHasher.Hash(command.newPassword);

        user.UpdatePassword(newPasswordHash, command.userId);

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
