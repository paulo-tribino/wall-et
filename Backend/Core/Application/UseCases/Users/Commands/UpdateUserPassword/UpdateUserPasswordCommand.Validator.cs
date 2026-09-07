using FluentValidation;

namespace Application.UseCases.Users.Commands.UpdateUserPassword;

internal sealed class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(command => command.userId)
            .NotEmpty().WithMessage("User ID cannot be empty");

        RuleFor(command => command.currentPassword)
            .NotEmpty().WithMessage("Current password cannot be empty");

        RuleFor(command => command.newPassword)
            .NotEmpty().WithMessage("New password cannot be empty")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters");
    }
}
