namespace Dyvenix.Auth.Core.Models;

public class Organization
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string SubscriptionTier { get; set; } = "Free";
    
    public ICollection<OrganizationUser> OrganizationUsers { get; set; } = new List<OrganizationUser>();
}

public class OrganizationUser
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public bool IsOwner { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationUser User { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
}
