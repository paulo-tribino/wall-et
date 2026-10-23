using Application.Abstractions.Messaging;
using Application.UseCases.Authentication.Commands.Login;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Authentication;

internal sealed class Login : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", LoginHandlerAsync)
            .WithTags(Tags.Authentication)
            .WithSummary("Log in and issue an access and refresh token")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();
    }

    private static async Task<IResult> LoginHandlerAsync(
        LoginRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.email,
            request.password);

        var result = await sender.SendAsync(
            command,
            cancellationToken);

        if (result.IsFailure)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(result.Value);
    }
}
