using Dyvenix.Auth.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.Auth.Core.Data;

public class AuthDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options)
        : base(options)
    {
    }

    // Multi-tenant tables
    public DbSet<Organization> Organizations { get; set; }
    public DbSet<OrganizationUser> OrganizationUsers { get; set; }
    
    // Multi-application tables
    public DbSet<Application> Applications { get; set; }
    public DbSet<ApplicationRole> ApplicationRoles { get; set; }
    public DbSet<ApplicationPermission> ApplicationPermissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    
    // User assignments
    public DbSet<UserApplicationRole> UserApplicationRoles { get; set; }
    public DbSet<UserApplicationPermission> UserApplicationPermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configure OpenIddict tables
        builder.UseOpenIddict();

        // ====================================
        // Organization Configuration
        // ====================================
        builder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Slug).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.SubscriptionTier).HasMaxLength(50);
        });

        // ====================================
        // OrganizationUser Configuration
        // ====================================
        builder.Entity<OrganizationUser>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.OrganizationId }).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.OrganizationMemberships)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Organization)
                .WithMany(o => o.OrganizationUsers)
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================================
        // Application Configuration
        // ====================================
        builder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.ClientId).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.ClientId).IsUnique();
        });

        // ====================================
        // ApplicationRole Configuration
        // ====================================
        builder.Entity<ApplicationRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasIndex(e => new { e.ApplicationId, e.Name }).IsUnique();
            
            entity.HasOne(e => e.Application)
                .WithMany(a => a.Roles)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================================
        // ApplicationPermission Configuration
        // ====================================
        builder.Entity<ApplicationPermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.HasIndex(e => new { e.ApplicationId, e.Name }).IsUnique();
            
            entity.HasOne(e => e.Application)
                .WithMany(a => a.Permissions)
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================================
        // RolePermission Configuration
        // ====================================
        builder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();
            
            entity.HasOne(e => e.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ====================================
        // UserApplicationRole Configuration
        // ====================================
        builder.Entity<UserApplicationRole>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.OrganizationId, e.ApplicationId, e.RoleId }).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.ApplicationRoles)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Organization)
                .WithMany()
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Application)
                .WithMany()
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Role)
                .WithMany(r => r.UserApplicationRoles)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================================
        // UserApplicationPermission Configuration
        // ====================================
        builder.Entity<UserApplicationPermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.OrganizationId, e.ApplicationId, e.PermissionId }).IsUnique();
            
            entity.HasOne(e => e.User)
                .WithMany(u => u.ApplicationPermissions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                
            entity.HasOne(e => e.Organization)
                .WithMany()
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Application)
                .WithMany()
                .HasForeignKey(e => e.ApplicationId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Permission)
                .WithMany(p => p.UserApplicationPermissions)
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ====================================
        // ApplicationUser Configuration
        // ====================================
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.HasOne(e => e.Organization)
                .WithMany()
                .HasForeignKey(e => e.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
