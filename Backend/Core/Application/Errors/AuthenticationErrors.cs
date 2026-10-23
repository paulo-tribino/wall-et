using SharedKernel;

namespace Application.Errors;

public static class AuthenticationErrors
{
    public static readonly Error InvalidCredentials = new(
        "Authentication.InvalidCredentials",
        "The email or password is incorrect.");

    public static readonly Error InvalidRefreshToken = new(
        "Authentication.InvalidRefreshToken",
        "The refresh token is invalid or expired.");
}
