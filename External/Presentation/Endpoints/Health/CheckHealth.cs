using Application.UseCases.Health.Queries.CheckHealth;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Health;

internal sealed class CheckHealth : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/health", CheckHealthHandlerAsync)
            .WithTags(Tags.Health)
            .AllowAnonymous();
    }

    private static async Task<IResult> CheckHealthHandlerAsync(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new CheckHealthQuery();

        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}
