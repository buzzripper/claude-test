using Microsoft.AspNetCore.Authorization;

namespace Dyvenix.Services.Common.Authorization;

public class AllPermissionsRequirement : IAuthorizationRequirement
{
    public string[] Permissions { get; }

    public AllPermissionsRequirement(params string[] permissions)
    {
        Permissions = permissions;
    }
}

public class AllPermissionsHandler : AuthorizationHandler<AllPermissionsRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AllPermissionsRequirement requirement)
    {
        var hasAllPermissions = requirement.Permissions.All(
            permission => context.User.HasClaim("permissions", permission));

        if (hasAllPermissions)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
