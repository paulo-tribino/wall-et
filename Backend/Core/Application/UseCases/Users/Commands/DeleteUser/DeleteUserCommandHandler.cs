using Application.Abstractions.Database;
using Application.Abstractions.Messaging;
using Application.Abstractions.Respositories;
using SharedKernel;

namespace Application.UseCases.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(
        DeleteUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.userId, cancellationToken);

        if (user is null)
        {
            return Result.Success();
        }

        user.Delete(command.deletedBy);

        _userRepository.Update(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
