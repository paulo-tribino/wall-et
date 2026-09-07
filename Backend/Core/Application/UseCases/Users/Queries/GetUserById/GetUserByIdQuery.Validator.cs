using FluentValidation;

namespace Application.UseCases.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(query => query.userId)
            .NotEmpty().WithMessage("User ID cannot be empty");
    }
}
