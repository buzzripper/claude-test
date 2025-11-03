namespace Dyvenix.Auth.Core.Models;

public class Application
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    
    public ICollection<ApplicationRole> Roles { get; set; } = new List<ApplicationRole>();
    public ICollection<ApplicationPermission> Permissions { get; set; } = new List<ApplicationPermission>();
}

public class ApplicationRole
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Application Application { get; set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserApplicationRole> UserApplicationRoles { get; set; } = new List<UserApplicationRole>();
}

public class ApplicationPermission
{
    public Guid Id { get; set; }
    public Guid ApplicationId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public Application Application { get; set; } = null!;
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public ICollection<UserApplicationPermission> UserApplicationPermissions { get; set; } = new List<UserApplicationPermission>();
}

public class RolePermission
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Guid PermissionId { get; set; }
    
    public ApplicationRole Role { get; set; } = null!;
    public ApplicationPermission Permission { get; set; } = null!;
}
