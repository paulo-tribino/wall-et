using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Presentation.Extensions;

public static class HttpContextExtensions
{
    public static Guid? GetUserId(this HttpContext httpContext)
    {
        if (httpContext == null)
        {
            return default;
        }

        var userId = httpContext.User.Claims.FirstOrDefault(c =>
                c.Type.Equals(JwtRegisteredClaimNames.Sub))?.Value;

        if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var parsedUserId))
        {
            return default;
        }

        return parsedUserId;
    }

    public static string? GetUserEmail(this HttpContext httpContext)
    {
        var userEmail = httpContext.User.Claims.FirstOrDefault(c =>
               c.Type.Equals(JwtRegisteredClaimNames.Email))?.Value;

        return userEmail;
    }
}
