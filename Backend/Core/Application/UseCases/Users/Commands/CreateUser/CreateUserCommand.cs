using Application.Abstractions.Messaging;

namespace Application.UseCases.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string name,
    string username,
    string email,
    string password,
    Guid? createdBy) : ICommand<Guid>;
