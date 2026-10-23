using FluentValidation;

namespace Application.UseCases.Authentication.Commands.Refresh;

internal sealed class RefreshCommandValidator : AbstractValidator<RefreshCommand>
{
    public RefreshCommandValidator()
    {
        RuleFor(command => command.refreshToken)
            .NotEmpty();
    }
}
