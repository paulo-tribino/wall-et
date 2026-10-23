using SharedKernel;

namespace Domain.Errors;

public static class RefreshTokenErrors
{
    public static readonly Error Revoked = new(
        "RefreshToken.Revoked",
        "The refresh token has already been revoked.");

    public static readonly Error Expired = new(
        "RefreshToken.Expired",
        "The refresh token has expired.");
}
