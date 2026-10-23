using Domain.Entities;

namespace Application.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken cancellationToken);

    Task RevokeFamilyAsync(
        Guid familyId,
        DateTime revokedAt,
        CancellationToken cancellationToken);
}
