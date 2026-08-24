using Application.Abstractions.Messaging;
using Application.UseCases.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Users;

internal sealed partial class CreateUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users", CreateUserHandlerAsync)
            .WithTags(Tags.Users)
            .WithSummary("Create a new user")
            .WithDescription("Creates a new user account with the provided name, username, email and password.")
            .Produces<Guid>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .AllowAnonymous();
    }

    private static async Task<IResult> CreateUserHandlerAsync(
        CreateUserRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.name,
            request.username,
            request.email,
            request.password,
            createdBy: null);

        var result = await sender.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/users/{result.Value}", result.Value)
            : Results.Conflict(result.Error);
    }
}
