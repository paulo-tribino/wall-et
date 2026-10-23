using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Authentication;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication;

internal sealed class RefreshTokenProvider : IRefreshTokenProvider
{
    private readonly JwtConfigurations _configurations;

    public RefreshTokenProvider(IOptions<JwtConfigurations> configurations)
    {
        _configurations = configurations.Value;
    }

    public string Generate()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    public string Hash(string token)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }

    public DateTime GetExpiresAt(DateTime createdAt)
    {
        return createdAt.AddDays(_configurations.RefreshTokenExpiresInDays);
    }
}
