using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.UseCases.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Users;

internal sealed class GetUserById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users/{id:guid}", GetUserByIdHandlerAsync)
            .WithTags(Tags.Users)
            .WithSummary("Get a user by ID")
            .WithDescription("Returns the user with the specified ID.")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }

    private static async Task<IResult> GetUserByIdHandlerAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var result = await sender.SendAsync(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Error);
    }
}
