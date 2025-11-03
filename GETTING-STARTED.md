# Quick Start Guide

## ✅ What's Included

This solution provides a **complete foundation** with:

### Fully Implemented:
- ✅ Solution structure with 11 projects
- ✅ All .csproj files configured for .NET 10 Preview
- ✅ Authorization framework in Services.Common
  - `RequirePermissionAttribute`
  - `RequireAllPermissionsAttribute`
  - Dynamic policy provider
  - ClaimsPrincipal extensions
- ✅ Multi-tenant, multi-application database schema
  - Organizations, Applications, Roles, Permissions
  - UserApplicationRoles, UserApplicationPermissions
- ✅ Auth models (ApplicationUser, Organization, Application, etc.)
- ✅ AuthDbContext with full EF configuration
- ✅ Comprehensive DataSeeder with sample data
- ✅ All Common projects (DTOs, Interfaces, Constants)

### To Be Completed:
- 🔲 Auth service implementations (follow patterns from previous version)
- 🔲 Auth controllers (UserManagementController, AuthorizationController)
- 🔲 Auth.Host Program.cs
- 🔲 App1 service implementations
- 🔲 App1.Host Program.cs
- 🔲 Notifications service implementations
- 🔲 Notifications.Host Program.cs
- 🔲 AppHost Program.cs

## 🚀 Next Steps

### 1. Install Prerequisites

```bash
# Install .NET 10 Preview
https://dotnet.microsoft.com/download/dotnet/10.0

# Install Docker Desktop
https://www.docker.com/products/docker-desktop/
```

### 2. Open Solution

```bash
# Open in Visual Studio 2026 Preview
Dyvenix.sln
```

### 3. Complete Service Implementations

Use the patterns from the previous solution:

**Auth Service:**
- Copy `UserManagementService.cs` from previous version
- Update to use `MultiTenantAuthService` for permission lookups
- Copy `AuthorizationController.cs` for OAuth token endpoint
- Update token generation to include organization and permissions

**App1 Service:**
- Copy `ProductService.cs`
- Add `organizationId` parameter to all methods
- Filter queries by `organizationId`

**Hosts:**
- Copy `Program.cs` files
- Update namespaces from `Dyvenix.*.Core` to `Dyvenix.*`
- Add authorization handler registration

### 4. Key Code Patterns

#### Auth Token Generation

```csharp
// In AuthorizationController.cs
private async Task<ClaimsPrincipal> CreatePrincipalAsync(
    ApplicationUser user, 
    OpenIddictRequest request)
{
    var identity = new ClaimsIdentity();
    
    // Add organization context
    if (user.OrganizationId.HasValue)
    {
        identity.AddClaim(new Claim("organization_id", user.OrganizationId.Value.ToString()));
    }
    
    // Get application from scope/client_id
    var application = await GetApplicationFromRequest(request);
    
    // Get user's permissions for THIS application in THEIR organization
    var permissions = await _context.UserApplicationRoles
        .Where(uar => uar.UserId == user.Id 
                   && uar.OrganizationId == user.OrganizationId.Value
                   && uar.ApplicationId == application.Id)
        .SelectMany(uar => uar.Role.RolePermissions)
        .Select(rp => rp.Permission.Name)
        .Distinct()
        .ToListAsync();
    
    // Add permissions to token
    foreach (var permission in permissions)
    {
        identity.AddClaim(new Claim("permissions", permission));
    }
    
    return new ClaimsPrincipal(identity);
}
```

#### Service Method with Multi-Tenant

```csharp
// In ProductService.cs
public async Task<List<ProductDto>> GetAllProductsAsync(Guid organizationId)
{
    return await _context.Products
        .Where(p => p.OrganizationId == organizationId)
        .ToListAsync();
}
```

#### Controller with Permission Check

```csharp
// In ProductsController.cs
using Dyvenix.ServiceDefaults.Authorization;
using Dyvenix.App1.Common.Constants;

[HttpGet]
[RequirePermission(Permissions.Products.Read)]
public async Task<IActionResult> GetAll()
{
    var orgId = User.GetOrganizationId()
        ?? throw new UnauthorizedAccessException("No organization context");
    
    var products = await _productService.GetAllProductsAsync(orgId);
    return Ok(products);
}
```

#### Program.cs with Authorization

```csharp
// In Auth.Host/Program.cs or App1.Host/Program.cs
using Dyvenix.ServiceDefaults.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services...

// Register authorization handlers
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AllPermissionsHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
```

## 📁 File Locations

### What's Complete:

```
src/
├── Services.Common/
│   ├── Extensions.cs                  ✅ Complete
│   └── Authorization/                  ✅ All files complete
│
├── Auth.Common/                        ✅ Complete
├── App1.Common/                        ✅ Complete
├── Notifications.Common/               ✅ Complete
│
└── Auth/
    ├── Models/                         ✅ Complete
    ├── Data/
    │   ├── AuthDbContext.cs            ✅ Complete
    │   └── DataSeeder.cs               ✅ Complete
    ├── Services/                       🔲 To be completed
    └── Controllers/                    🔲 To be completed
```

### Reference Previous Solution

The previous solution (with `Dyvenix.*.Core` naming) has complete implementations for:
- Auth services and controllers
- App1 services and controllers
- Notifications services and controllers
- All Program.cs files
- AppHost orchestration

**Copy those files and:**
1. Update namespaces (remove `.Core` suffix)
2. Add multi-tenant support (organizationId parameters)
3. Update authorization to use `[RequirePermission]` attributes

## 🎓 Key Differences to Remember

1. **No "Core" in namespaces**
   - Old: `Dyvenix.Auth.Core.Services`
   - New: `Dyvenix.Auth.Services` (assembly is still Dyvenix.Auth.Core)

2. **Separate Common projects**
   - DTOs, Interfaces, Constants in `*.Common` projects
   - Services reference their own Common project

3. **Services.Common has authorization**
   - Import: `using Dyvenix.ServiceDefaults.Authorization;`
   - Use: `[RequirePermission(Permissions.Products.Read)]`

4. **Multi-tenant everywhere**
   - All queries filter by `organizationId`
   - Get from: `User.GetOrganizationId()`
   - Stored in token: `organization_id` claim

5. **Permissions scoped by application**
   - Token for App1 has App1 permissions only
   - Token for Notifications has Notification permissions only
   - Separate tokens per application

## 🧪 Testing After Completion

### 1. Start Docker Desktop

### 2. Run AppHost

```bash
dotnet run --project src/AppHost
```

### 3. Apply Migrations

```bash
cd src/Auth.Host
dotnet ef database update

cd ../App1.Host
dotnet ef database update
```

### 4. Test Token Generation

```http
POST http://localhost:5001/connect/token

grant_type=password
&client_id=postman-client
&client_secret=postman-secret-2024
&username=admin@dyvenix.com
&password=Admin123!
&scope=app1-api
```

### 5. Test Protected Endpoint

```http
GET http://localhost:5002/api/products
Authorization: Bearer {token}
```

## 📚 Resources

- Previous solution for reference implementations
- README.md for complete architecture documentation
- Comments in DataSeeder.cs for sample data structure
- Authorization classes in Services.Common for usage examples

## ❓ Questions?

Check the comprehensive README.md for:
- Complete database schema
- Authorization framework details
- Multi-tenant architecture explanation
- Troubleshooting guide

---

**You have a solid foundation. Just complete the service implementations following the established patterns!**
