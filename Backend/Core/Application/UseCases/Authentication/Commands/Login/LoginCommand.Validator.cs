using FluentValidation;

namespace Application.UseCases.Authentication.Commands.Login;

internal sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(command => command.email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(command => command.password)
            .NotEmpty();
    }
}
