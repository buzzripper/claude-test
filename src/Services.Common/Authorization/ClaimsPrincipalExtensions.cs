using System.Security.Claims;

namespace Dyvenix.Services.Common.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static bool HasPermission(this ClaimsPrincipal user, string permission)
    {
        return user.HasClaim("permissions", permission);
    }

    public static bool HasAnyPermission(this ClaimsPrincipal user, params string[] permissions)
    {
        return permissions.Any(p => user.HasClaim("permissions", p));
    }

    public static bool HasAllPermissions(this ClaimsPrincipal user, params string[] permissions)
    {
        return permissions.All(p => user.HasClaim("permissions", p));
    }

    public static List<string> GetPermissions(this ClaimsPrincipal user)
    {
        return user.FindAll("permissions").Select(c => c.Value).ToList();
    }

    public static Guid? GetOrganizationId(this ClaimsPrincipal user)
    {
        var orgClaim = user.FindFirst("organization_id");
        if (orgClaim != null && Guid.TryParse(orgClaim.Value, out var orgId))
        {
            return orgId;
        }
        return null;
    }

    public static bool IsGlobalAdmin(this ClaimsPrincipal user)
    {
        return user.HasClaim("is_global_admin", "true");
    }
}
