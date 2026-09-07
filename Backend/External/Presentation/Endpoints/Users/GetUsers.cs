using Application.Abstractions.Messaging;
using Application.Dtos;
using Application.Dtos.Base;
using Application.UseCases.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Users;

internal sealed class GetUsers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", GetUsersHandlerAsync)
            .WithTags(Tags.Users)
            .WithSummary("Get a paged list of users")
            .WithDescription("Returns a paginated list of users. Defaults to page 1 with 20 items per page.")
            .Produces<PagedList<UserDto>>(StatusCodes.Status200OK)
            .AllowAnonymous();
    }

    private static async Task<IResult> GetUsersHandlerAsync(
        ISender sender,
        CancellationToken cancellationToken,
        [FromQuery] int page,
        [FromQuery] int pageSize)
    {
        var query = new GetUsersQuery(page, pageSize);

        var result = await sender.SendAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return Results.BadRequest(result.Error);
        }

        return Results.Ok(result.Value);
    }
}
