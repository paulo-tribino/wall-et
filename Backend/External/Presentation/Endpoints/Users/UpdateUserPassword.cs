using Application.Abstractions.Messaging;
using Application.UseCases.Users.Commands.UpdateUserPassword;
using Domain.Errors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;
using SharedKernel.Extensions;

namespace Presentation.Endpoints.Users;

internal sealed class UpdateUserPassword : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/users/{id:guid}/password", UpdateUserPasswordHandlerAsync)
            .WithTags(Tags.Users)
            .WithSummary("Update user password")
            .WithDescription("Updates the password of the specified user. The current password must be provided for verification.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .AllowAnonymous();
    }

    private static async Task<IResult> UpdateUserPasswordHandlerAsync(
        Guid id,
        UpdateUserPasswordRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserPasswordCommand(
            id,
            request.currentPassword,
            request.newPassword);

        var result = await sender.SendAsync(command, cancellationToken);

        if (!result.IsSuccess)
        {
            return result.Error.Code.EqualsIgnoreCase(UserErrors.NotFound.Code)
                ? Results.NotFound(result.Error)
                : Results.BadRequest(result.Error);
        }

        return Results.NoContent();
    }
}
