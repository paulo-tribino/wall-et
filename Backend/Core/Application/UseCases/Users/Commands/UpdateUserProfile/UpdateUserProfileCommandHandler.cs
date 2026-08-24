using Application.Abstractions.Database;
using Application.Abstractions.Messaging;
using Application.Abstractions.Respositories;
using Domain.Errors;
using SharedKernel;

namespace Application.UseCases.Users.Commands.UpdateUserProfile;

internal sealed class UpdateUserProfileCommandHandler : ICommandHandler<UpdateUserProfileCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserProfileCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(
        UpdateUserProfileCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.userId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (user.Username != command.username &&
            await _userRepository.ExistsByUsernameAsync(command.username, cancellationToken))
        {
            return Result.Failure(UserErrors.UsernameAlreadyInUse);
        }

        if (user.Email != command.email &&
            await _userRepository.ExistsByEmailAsync(command.email, cancellationToken))
        {
            return Result.Failure(UserErrors.EmailAlreadyInUse);
        }

        user.UpdateProfile(
            command.name,
            command.username,
            command.email,
            command.updatedBy);

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
