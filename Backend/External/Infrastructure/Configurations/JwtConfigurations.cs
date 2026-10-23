namespace Infrastructure.Configurations;

public class JwtConfigurations
{
    public required string Issuer { get; init; }

    public required string Audience { get; init; }

    public required string SecretKey { get; init; }

    public int AccessTokenExpiresInMinutes { get; init; }

    public int RefreshTokenExpiresInDays { get; init; }
}
