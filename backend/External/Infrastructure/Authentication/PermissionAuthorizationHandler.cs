using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Enums;
using Infrastructure.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        if (!TryGetUserId(context.User, out _))
        {
            return;
        }

        if (!TryGetPermissionId(requirement.Permission, out var permissionId))
        {
            return;
        }

        if (HasPermissionClaim(context.User, permissionId))
        {
            context.Succeed(requirement);
        }
    }

    private static bool TryGetUserId(ClaimsPrincipal user, out Guid userId)
    {
        var userIdValue = user.Claims
            .FirstOrDefault(c => c.Type.Equals(JwtRegisteredClaimNames.Sub))?.Value;

        return Guid.TryParse(userIdValue, out userId);
    }

    private static bool TryGetPermissionId(string permissionName, out string permissionId)
    {
        if (!Enum.TryParse(permissionName, out Permission permission))
        {
            permissionId = string.Empty;
            return false;
        }

        permissionId = ((int)permission).ToString();

        return true;
    }

    private static bool HasPermissionClaim(ClaimsPrincipal user, string permissionId)
    {
        var hasPermission = user.Claims.Any(c =>
            c.Type == ClaimConstants.PermissionClaimType &&
            c.Value == permissionId);

        return hasPermission;
    }
}
