using Application.Abstractions.Messaging;
using Application.UseCases.Authentication.Commands.Refresh;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Authentication;

internal sealed class Refresh : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/refresh", RefreshHandlerAsync)
            .WithTags(Tags.Authentication)
            .WithSummary("Rotate a refresh token")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();
    }

    private static async Task<IResult> RefreshHandlerAsync(
        RefreshRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RefreshCommand(request.refreshToken);

        var result = await sender.SendAsync(
            command,
            cancellationToken);

        return result.IsFailure
            ? Results.Unauthorized()
            : Results.Ok(result.Value);
    }
}
