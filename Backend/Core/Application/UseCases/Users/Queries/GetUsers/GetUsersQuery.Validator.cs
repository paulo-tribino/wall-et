using FluentValidation;

namespace Application.UseCases.Users.Queries.GetUsers;

internal sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(query => query.page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(query => query.pageSize)
            .GreaterThan(0).WithMessage("Page size must be greater than 0")
            .LessThanOrEqualTo(100).WithMessage("Page size must not exceed 100");
    }
}
