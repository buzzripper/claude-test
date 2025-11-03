namespace Dyvenix.Auth.Core.Models;

public class UserApplicationRole
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid RoleId { get; set; }
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationUser User { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
    public Application Application { get; set; } = null!;
    public ApplicationRole Role { get; set; } = null!;
}

public class UserApplicationPermission
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid PermissionId { get; set; }
    public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationUser User { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
    public Application Application { get; set; } = null!;
    public ApplicationPermission Permission { get; set; } = null!;
}
