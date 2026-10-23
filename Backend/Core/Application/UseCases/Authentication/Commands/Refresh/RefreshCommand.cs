using Application.Abstractions.Messaging;
using Application.Dtos;

namespace Application.UseCases.Authentication.Commands.Refresh;

public sealed record RefreshCommand(string refreshToken)
    : ICommand<AuthenticationTokenDto>;
