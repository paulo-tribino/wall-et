using FluentValidation;

namespace Application.UseCases.Users.Commands.CreateUser;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.name)
            .NotEmpty().WithMessage("Name cannot be empty");

        RuleFor(command => command.username)
            .NotEmpty().WithMessage("Username cannot be empty");

        RuleFor(command => command.email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Invalid email address");

        RuleFor(command => command.password)
            .NotEmpty().WithMessage("Password cannot be empty")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters");
    }
}
