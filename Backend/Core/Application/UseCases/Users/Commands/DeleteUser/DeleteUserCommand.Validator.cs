using FluentValidation;

namespace Application.UseCases.Users.Commands.DeleteUser;

internal sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.userId)
            .NotEmpty().WithMessage("User ID cannot be empty");
    }
}
