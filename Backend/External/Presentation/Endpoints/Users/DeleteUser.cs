using Application.Abstractions.Messaging;
using Application.UseCases.Users.Commands.DeleteUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Constants;

namespace Presentation.Endpoints.Users;

internal sealed class DeleteUser : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/users/{id:guid}", DeleteUserHandlerAsync)
            .WithTags(Tags.Users)
            .WithSummary("Delete a user")
            .WithDescription("Soft-deletes the specified user.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous();
    }

    private static async Task<IResult> DeleteUserHandlerAsync(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(id, deletedBy: null);

        var result = await sender.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.NotFound(result.Error);
    }
}
