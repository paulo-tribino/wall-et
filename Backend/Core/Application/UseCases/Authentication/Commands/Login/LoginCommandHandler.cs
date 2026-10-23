using Application.Abstractions.Authentication;
using Application.Abstractions.Database;
using Application.Abstractions.Hashers;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Dtos;
using Application.Errors;
using Domain.Entities;
using SharedKernel;

namespace Application.UseCases.Authentication.Commands.Login;

internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthenticationTokenDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRefreshTokenProvider _refreshTokenProvider;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider,
        IRefreshTokenProvider refreshTokenProvider,
        IRefreshTokenRepository refreshTokenRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _refreshTokenProvider = refreshTokenProvider;
        _refreshTokenRepository = refreshTokenRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AuthenticationTokenDto>> HandleAsync(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(command.email, cancellationToken);

        if (user is null || !_passwordHasher.Verify(command.password, user.PasswordHash))
        {
            return Result.Failure<AuthenticationTokenDto>(AuthenticationErrors.InvalidCredentials);
        }

        var now = DateTime.UtcNow;

        var refreshToken = _refreshTokenProvider.Generate();

        var refreshTokenEntity = RefreshToken.CreateSession(
            user.Id,
            _refreshTokenProvider.Hash(refreshToken),
            createdAt: now,
            _refreshTokenProvider.GetExpiresAt(now));

        await _refreshTokenRepository.AddAsync(refreshTokenEntity, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthenticationTokenDto(
            _jwtProvider.Generate(user),
            refreshToken,
            _jwtProvider.ExpiresInSeconds));
    }
}
