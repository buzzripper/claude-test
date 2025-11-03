using Microsoft.AspNetCore.Authorization;

namespace Dyvenix.Services.Common.Authorization;

/// <summary>
/// Authorization attribute that requires one or more permissions (OR logic).
/// User needs at least ONE of the specified permissions.
/// </summary>
public class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(params string[] permissions)
    {
        Permissions = permissions;
        Policy = $"Permission:{string.Join(",", permissions)}";
    }

    public string[] Permissions { get; }
}
