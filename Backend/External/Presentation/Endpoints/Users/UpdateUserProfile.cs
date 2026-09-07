using Application.Abstractions.Messaging;
using Application.Errors;
using Application.UseCases.Users.Commands.UpdateUserProfile;
using Domain.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;
using SharedKernel.Extensions;

namespace Presentation.Endpoints.Users;

internal sealed class UpdateUserProfile : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{id:guid}/profile", UpdateUserProfileHandlerAsync)
            .WithTags(Tags.Users)
            .WithSummary("Update user profile")
            .WithDescription("Updates the name, username and email of the specified user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .AllowAnonymous();
    }

    private static async Task<IResult> UpdateUserProfileHandlerAsync(
        Guid id,
        UpdateUserProfileRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserProfileCommand(
            id,
            request.name,
            request.username,
            request.email,
            updatedBy: null);

        var result = await sender.SendAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return
                result.Error.Code.EqualsIgnoreCase(ValidationErrors.BadRequest(string.Empty).Code)
                ? Results.BadRequest(result.Error)
                : result.Error.Code.EqualsIgnoreCase(UserErrors.NotFound.Code)
                    ? Results.NotFound(result.Error)
                    : Results.Conflict(result.Error);
        }

        return Results.NoContent();
    }
}
