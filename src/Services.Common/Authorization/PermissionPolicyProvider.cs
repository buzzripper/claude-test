using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Dyvenix.Services.Common.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return _fallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return _fallbackPolicyProvider.GetFallbackPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission:", StringComparison.OrdinalIgnoreCase))
        {
            var permissionsString = policyName.Substring("Permission:".Length);
            var permissions = permissionsString.Split(',');
            
            var policy = new AuthorizationPolicyBuilder();
            foreach (var permission in permissions)
            {
                policy.AddRequirements(new PermissionRequirement(permission));
            }
            
            return Task.FromResult<AuthorizationPolicy?>(policy.Build());
        }
        
        if (policyName.StartsWith("AllPermissions:", StringComparison.OrdinalIgnoreCase))
        {
            var permissionsString = policyName.Substring("AllPermissions:".Length);
            var permissions = permissionsString.Split(',');
            
            var policy = new AuthorizationPolicyBuilder()
                .AddRequirements(new AllPermissionsRequirement(permissions))
                .Build();

            return Task.FromResult<AuthorizationPolicy?>(policy);
        }

        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
