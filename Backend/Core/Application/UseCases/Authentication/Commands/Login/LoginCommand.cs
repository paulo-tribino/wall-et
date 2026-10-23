using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.UseCases.Authentication.Commands.Login;

public sealed record LoginCommand(string email, string password)
    : ICommand<AuthenticationTokenDto>;
