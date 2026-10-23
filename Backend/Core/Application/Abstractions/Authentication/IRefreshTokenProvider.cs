namespace Application.Abstractions.Authentication;

public interface IRefreshTokenProvider
{
    string Generate();

    string Hash(string token);

    DateTime GetExpiresAt(DateTime createdAt);
}
