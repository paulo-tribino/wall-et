using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
using Domain.Entities;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly JwtConfigurations _configurations;

    public JwtProvider(IOptions<JwtConfigurations> configurations)
    {
        _configurations = configurations.Value;
    }

    public int ExpiresInSeconds => _configurations.AccessTokenExpiresInMinutes * 60;

    public string Generate(User user)
    {
        var claims = BuildClaims(user);

        var signingCredentials = BuildSigningCredentials();

        return BuildTokenValue(claims, signingCredentials);
    }

    private static IEnumerable<Claim> BuildClaims(User user)
    {
        return
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.Name)
        ];
    }

    private SigningCredentials BuildSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurations.SecretKey)),
            SecurityAlgorithms.HmacSha256);
    }

    private string BuildTokenValue(IEnumerable<Claim> claims, SigningCredentials signingCredentials)
    {
        var token = new JwtSecurityToken(
            _configurations.Issuer,
            _configurations.Audience,
            claims,
            notBefore: null,
            expires: DateTime.UtcNow.AddMinutes(_configurations.AccessTokenExpiresInMinutes),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
