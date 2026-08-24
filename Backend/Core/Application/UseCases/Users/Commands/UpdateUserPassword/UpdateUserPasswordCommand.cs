using Application.Abstractions.Messaging;

namespace Application.UseCases.Users.Commands.UpdateUserPassword;

public sealed record UpdateUserPasswordCommand(
    Guid userId,
    string currentPassword,
    string newPassword) : ICommand;
