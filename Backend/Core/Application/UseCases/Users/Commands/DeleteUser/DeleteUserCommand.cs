using Application.Abstractions.Messaging;

namespace Application.UseCases.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(
    Guid userId,
    Guid? deletedBy) : ICommand;
