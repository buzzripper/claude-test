using Microsoft.AspNetCore.Authorization;

namespace Dyvenix.Services.Common.Authorization;

/// <summary>
/// Authorization attribute that requires ALL specified permissions (AND logic).
/// User must have ALL of the specified permissions.
/// </summary>
public class RequireAllPermissionsAttribute : AuthorizeAttribute
{
    public RequireAllPermissionsAttribute(params string[] permissions)
    {
        Permissions = permissions;
        Policy = $"AllPermissions:{string.Join(",", permissions)}";
    }

    public string[] Permissions { get; }
}
