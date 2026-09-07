using FluentValidation;

namespace Application.UseCases.Users.Commands.UpdateUserProfile;

internal sealed class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(command => command.userId)
            .NotEmpty().WithMessage("User ID cannot be empty");

        RuleFor(command => command.name)
            .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(command => command.username)
            .NotEmpty().WithMessage("Username cannot be empty");

        RuleFor(command => command.email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");
    }
}
