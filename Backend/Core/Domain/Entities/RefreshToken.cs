using SharedKernel;
using Domain.Errors;

namespace Domain.Entities;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Guid FamilyId { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;

    public static RefreshToken CreateSession(
        Guid userId,
        string tokenHash,
        DateTime createdAt,
        DateTime expiresAt)
    {
        return Create(
            userId,
            Guid.CreateVersion7(),
            tokenHash,
            createdAt,
            expiresAt);
    }

    public Result<RefreshToken> Rotate(
        string tokenHash,
        DateTime rotatedAt,
        DateTime expiresAt)
    {
        if (IsRevoked)
        {
            // it means some malicious client might have catch a refresh token and tried to generate a new access token.
            return Result.Failure<RefreshToken>(RefreshTokenErrors.Revoked);
        }

        if (ExpiresAt <= rotatedAt)
        {
            return Result.Failure<RefreshToken>(RefreshTokenErrors.Expired);
        }

        Revoke(rotatedAt);

        var replacement = Create(
            UserId,
            FamilyId,
            tokenHash,
            rotatedAt,
            expiresAt);

        return Result.Success(replacement);
    }

    public void Revoke(DateTime revokedAt)
    {
        RevokedAt ??= revokedAt;
    }

    public static RefreshToken Create(
        Guid userId,
        Guid familyId,
        string tokenHash,
        DateTime createdAt,
        DateTime expiresAt)
    {
        return new RefreshToken
        {
            Id = Guid.CreateVersion7(),
            UserId = userId,
            FamilyId = familyId,
            TokenHash = tokenHash,
            CreatedAt = createdAt,
            ExpiresAt = expiresAt
        };
    }
}
