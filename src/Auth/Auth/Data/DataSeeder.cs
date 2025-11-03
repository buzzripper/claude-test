using Dyvenix.Auth.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace Dyvenix.Auth.Core.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<AuthDbContext>>();
        logger.LogInformation("Starting data seeding...");

        await SeedOrganizationsAsync(serviceProvider);
        await SeedApplicationsAndPermissionsAsync(serviceProvider);
        await SeedUsersAsync(serviceProvider);
        await SeedOpenIddictDataAsync(serviceProvider);

        logger.LogInformation("Data seeding completed.");
    }

    private static async Task SeedOrganizationsAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AuthDbContext>();
        
        if (await context.Organizations.AnyAsync())
            return;

        var organizations = new[]
        {
            new Organization
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Acme Corporation",
                Slug = "acme-corp",
                SubscriptionTier = "Enterprise"
            },
            new Organization
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Tech Startup Inc",
                Slug = "tech-startup",
                SubscriptionTier = "Pro"
            }
        };

        context.Organizations.AddRange(organizations);
        await context.SaveChangesAsync();
    }

    private static async Task SeedApplicationsAndPermissionsAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<AuthDbContext>();
        
        if (await context.Applications.AnyAsync())
            return;

        var authApp = new Application
        {
            Id = Guid.Parse("AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA"),
            Name = "Auth System",
            ClientId = "auth-api"
        };

        var app1 = new Application
        {
            Id = Guid.Parse("BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB"),
            Name = "App1 System",
            ClientId = "app1-api"
        };

        var notificationsApp = new Application
        {
            Id = Guid.Parse("CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC"),
            Name = "Notifications System",
            ClientId = "notifications-api"
        };

        context.Applications.AddRange(authApp, app1, notificationsApp);
        await context.SaveChangesAsync();

        var authPermissions = new[]
        {
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = authApp.Id, Name = "users.read", DisplayName = "Read Users" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = authApp.Id, Name = "users.create", DisplayName = "Create Users" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = authApp.Id, Name = "users.update", DisplayName = "Update Users" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = authApp.Id, Name = "users.delete", DisplayName = "Delete Users" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = authApp.Id, Name = "users.manage", DisplayName = "Manage Users" }
        };

        var app1Permissions = new[]
        {
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "products.read", DisplayName = "Read Products" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "products.create", DisplayName = "Create Products" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "products.update", DisplayName = "Update Products" },
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "products.delete", DisplayName = "Delete Products" }
        };

        var notifPermissions = new[]
        {
            new ApplicationPermission { Id = Guid.NewGuid(), ApplicationId = notificationsApp.Id, Name = "notifications.send", DisplayName = "Send Notifications" }
        };

        context.ApplicationPermissions.AddRange(authPermissions);
        context.ApplicationPermissions.AddRange(app1Permissions);
        context.ApplicationPermissions.AddRange(notifPermissions);
        await context.SaveChangesAsync();

        var app1AdminRole = new ApplicationRole { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "Admin", Description = "App1 Administrator" };
        var app1ManagerRole = new ApplicationRole { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "Manager", Description = "App1 Manager" };
        var app1UserRole = new ApplicationRole { Id = Guid.NewGuid(), ApplicationId = app1.Id, Name = "User", Description = "App1 Regular User" };

        context.ApplicationRoles.AddRange(app1AdminRole, app1ManagerRole, app1UserRole);
        await context.SaveChangesAsync();

        var app1AdminRolePerms = app1Permissions.Select(p => new RolePermission { Id = Guid.NewGuid(), RoleId = app1AdminRole.Id, PermissionId = p.Id });
        var app1ManagerRolePerms = app1Permissions.Where(p => p.Name != "products.delete").Select(p => new RolePermission { Id = Guid.NewGuid(), RoleId = app1ManagerRole.Id, PermissionId = p.Id });
        var app1UserRolePerms = app1Permissions.Where(p => p.Name == "products.read").Select(p => new RolePermission { Id = Guid.NewGuid(), RoleId = app1UserRole.Id, PermissionId = p.Id });

        context.RolePermissions.AddRange(app1AdminRolePerms);
        context.RolePermissions.AddRange(app1ManagerRolePerms);
        context.RolePermissions.AddRange(app1UserRolePerms);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<AuthDbContext>();

        var acmeOrg = await context.Organizations.FirstAsync(o => o.Slug == "acme-corp");
        var app1 = await context.Applications.FirstAsync(a => a.ClientId == "app1-api");
        var app1AdminRole = await context.ApplicationRoles.FirstAsync(r => r.ApplicationId == app1.Id && r.Name == "Admin");
        var app1ManagerRole = await context.ApplicationRoles.FirstAsync(r => r.ApplicationId == app1.Id && r.Name == "Manager");
        var app1UserRole = await context.ApplicationRoles.FirstAsync(r => r.ApplicationId == app1.Id && r.Name == "User");

        if (await userManager.FindByEmailAsync("admin@dyvenix.com") == null)
        {
            var adminUser = new ApplicationUser { UserName = "admin", Email = "admin@dyvenix.com", EmailConfirmed = true, FirstName = "Admin", LastName = "User", OrganizationId = acmeOrg.Id, IsGlobalAdmin = true };
            await userManager.CreateAsync(adminUser, "Admin123!");
            context.OrganizationUsers.Add(new OrganizationUser { Id = Guid.NewGuid(), UserId = adminUser.Id, OrganizationId = acmeOrg.Id, IsOwner = true });
            context.UserApplicationRoles.Add(new UserApplicationRole { Id = Guid.NewGuid(), UserId = adminUser.Id, OrganizationId = acmeOrg.Id, ApplicationId = app1.Id, RoleId = app1AdminRole.Id });
            await context.SaveChangesAsync();
        }

        if (await userManager.FindByEmailAsync("manager@dyvenix.com") == null)
        {
            var managerUser = new ApplicationUser { UserName = "manager", Email = "manager@dyvenix.com", EmailConfirmed = true, FirstName = "Manager", LastName = "User", OrganizationId = acmeOrg.Id };
            await userManager.CreateAsync(managerUser, "Manager123!");
            context.OrganizationUsers.Add(new OrganizationUser { Id = Guid.NewGuid(), UserId = managerUser.Id, OrganizationId = acmeOrg.Id, IsOwner = false });
            context.UserApplicationRoles.Add(new UserApplicationRole { Id = Guid.NewGuid(), UserId = managerUser.Id, OrganizationId = acmeOrg.Id, ApplicationId = app1.Id, RoleId = app1ManagerRole.Id });
            await context.SaveChangesAsync();
        }

        if (await userManager.FindByEmailAsync("user@dyvenix.com") == null)
        {
            var regularUser = new ApplicationUser { UserName = "user", Email = "user@dyvenix.com", EmailConfirmed = true, FirstName = "Regular", LastName = "User", OrganizationId = acmeOrg.Id };
            await userManager.CreateAsync(regularUser, "User123!");
            context.OrganizationUsers.Add(new OrganizationUser { Id = Guid.NewGuid(), UserId = regularUser.Id, OrganizationId = acmeOrg.Id, IsOwner = false });
            context.UserApplicationRoles.Add(new UserApplicationRole { Id = Guid.NewGuid(), UserId = regularUser.Id, OrganizationId = acmeOrg.Id, ApplicationId = app1.Id, RoleId = app1UserRole.Id });
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedOpenIddictDataAsync(IServiceProvider serviceProvider)
    {
        var applicationManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
        var scopeManager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await scopeManager.FindByNameAsync("app1-api") == null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = "app1-api", DisplayName = "App1 API", Resources = { "app1-api" } });
        }

        if (await scopeManager.FindByNameAsync("notifications-api") == null)
        {
            await scopeManager.CreateAsync(new OpenIddictScopeDescriptor { Name = "notifications-api", DisplayName = "Notifications API", Resources = { "notifications-api" } });
        }

        if (await applicationManager.FindByClientIdAsync("postman-client") == null)
        {
            await applicationManager.CreateAsync(new OpenIddictApplicationDescriptor
            {
                ClientId = "postman-client",
                ClientSecret = "postman-secret-2024",
                DisplayName = "Postman Testing Client",
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.Password,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    OpenIddictConstants.Permissions.Prefixes.Scope + "app1-api",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "notifications-api"
                }
            });
        }
    }
}
