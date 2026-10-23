using Application.Abstractions.Authentication;
using Application.Abstractions.Database;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Dtos;
using Application.Errors;
using SharedKernel;

namespace Application.UseCases.Authentication.Commands.Refresh;

internal sealed class RefreshCommandHandler : ICommandHandler<RefreshCommand, AuthenticationTokenDto>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IRefreshTokenProvider _refreshTokenProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserRepository _userRepository;

    public RefreshCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        IRefreshTokenProvider refreshTokenProvider,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IUserRepository userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _refreshTokenProvider = refreshTokenProvider;
        _unitOfWork = unitOfWork;
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }

    public async Task<Result<AuthenticationTokenDto>> HandleAsync(
        RefreshCommand command,
        CancellationToken cancellationToken)
    {
        var tokenHash = _refreshTokenProvider.Hash(command.refreshToken);

        var currentToken = await _refreshTokenRepository.GetByHashAsync(tokenHash, cancellationToken);
        if (currentToken is null)
        {
            return Result.Failure<AuthenticationTokenDto>(AuthenticationErrors.InvalidRefreshToken);
        }

        var user = await _userRepository.GetByIdAsync(currentToken.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<AuthenticationTokenDto>(AuthenticationErrors.InvalidRefreshToken);
        }

        var now = DateTime.UtcNow;

        var rawReplacement = _refreshTokenProvider.Generate();

        var rotation = currentToken.Rotate(
            _refreshTokenProvider.Hash(rawReplacement),
            now,
            _refreshTokenProvider.GetExpiresAt(now));

        if (rotation.IsFailure)
        {
            await _refreshTokenRepository.RevokeFamilyAsync(
                currentToken.FamilyId,
                now,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Failure<AuthenticationTokenDto>(AuthenticationErrors.InvalidRefreshToken);
        }

        var replacement = rotation.Value!;

        await _refreshTokenRepository.AddAsync(replacement, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new AuthenticationTokenDto(
            AccessToken: _jwtProvider.Generate(user),
            RefreshToken: rawReplacement,
            ExpiresIn: _jwtProvider.ExpiresInSeconds));
    }
}
