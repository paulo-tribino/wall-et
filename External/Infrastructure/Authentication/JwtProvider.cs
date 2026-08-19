using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Abstractions.Authentication;
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

    public string Generate()
    {
        var claims = BuildClaims();

        var signingCredentials = BuildSigningCredentials();

        return BuildTokenValue(claims, signingCredentials);
    }

    private static IEnumerable<Claim> BuildClaims()
    {
        //new Claim[]
        //{
        //    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        //    new(JwtRegisteredClaimNames.Email, user.Email),
        //    new(JwtRegisteredClaimNames.Name, user.FirstName),
        //};

        return Enumerable.Empty<Claim>();
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
            DateTime.UtcNow.AddHours(_configurations.ExpiresInHours),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
