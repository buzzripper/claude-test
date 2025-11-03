using Microsoft.AspNetCore.Identity;

namespace Dyvenix.Auth.Core.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? OrganizationId { get; set; }
    public bool IsGlobalAdmin { get; set; }
    
    // Navigation properties
    public Organization? Organization { get; set; }
    public ICollection<OrganizationUser> OrganizationMemberships { get; set; } = new List<OrganizationUser>();
    public ICollection<UserApplicationRole> ApplicationRoles { get; set; } = new List<UserApplicationRole>();
    public ICollection<UserApplicationPermission> ApplicationPermissions { get; set; } = new List<UserApplicationPermission>();
}
