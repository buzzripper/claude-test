# Permission System Guide

## Overview

Permissions are hard-coded capabilities in your application that control what actions users can perform.

## Defining Permissions

### In Common Projects

```csharp
// App1.Common/Constants/Permissions.cs
namespace Dyvenix.App1.Common.Constants;

public static class Permissions
{
    public static class Products
    {
        public const string Read = "products.read";
        public const string Create = "products.create";
        public const string Update = "products.update";
        public const string Delete = "products.delete";
    }
}
```

### In Database (DataSeeder)

```csharp
var permissions = new[]
{
    new ApplicationPermission 
    { 
        ApplicationId = app1.Id, 
        Name = "products.read", 
        DisplayName = "Read Products",
        Description = "View product list and details"
    },
    new ApplicationPermission 
    { 
        ApplicationId = app1.Id, 
        Name = "products.create", 
        DisplayName = "Create Products",
        Description = "Add new products"
    }
};
```

## Using Permissions

### In Controllers

```csharp
using Dyvenix.ServiceDefaults.Authorization;
using Dyvenix.App1.Common.Constants;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    // Single permission
    [HttpGet]
    [RequirePermission(Permissions.Products.Read)]
    public IActionResult GetAll() { }

    // Multiple permissions (OR logic)
    [HttpPost]
    [RequirePermission(Permissions.Products.Create, Permissions.Products.Update)]
    public IActionResult CreateOrUpdate() { }

    // Multiple permissions (AND logic)
    [HttpDelete]
    [RequireAllPermissions(Permissions.Products.Delete, Permissions.Products.Update)]
    public IActionResult Delete() { }
}
```

### Imperative Checks

```csharp
using Dyvenix.ServiceDefaults.Authorization;

public IActionResult ComplexAction()
{
    // Check single permission
    if (!User.HasPermission(Permissions.Products.Read))
        return Forbid();

    // Check multiple permissions (OR)
    if (!User.HasAnyPermission(
        Permissions.Products.Create, 
        Permissions.Products.Update))
        return Forbid();

    // Check multiple permissions (AND)
    if (!User.HasAllPermissions(
        Permissions.Products.Read,
        Permissions.Products.Update))
        return Forbid();

    // Get all permissions
    var userPermissions = User.GetPermissions();
    
    // Process...
}
```

## Creating Roles

Roles are collections of permissions that can be assigned to users.

### Via Admin UI (To Be Implemented)

```csharp
// Create role
var role = new ApplicationRole
{
    ApplicationId = app1.Id,
    Name = "Product Manager",
    Description = "Manages products"
};

// Assign permissions to role
var rolePermissions = new[]
{
    new RolePermission 
    { 
        RoleId = role.Id, 
        PermissionId = productsReadId 
    },
    new RolePermission 
    { 
        RoleId = role.Id, 
        PermissionId = productsCreateId 
    },
    new RolePermission 
    { 
        RoleId = role.Id, 
        PermissionId = productsUpdateId 
    }
};
```

### Assigning Roles to Users

```csharp
var userRole = new UserApplicationRole
{
    UserId = user.Id,
    OrganizationId = user.OrganizationId,
    ApplicationId = app1.Id,
    RoleId = roleId
};
```

## Best Practices

1. **Define permissions as constants** - Use `Permissions.*.Read` not magic strings
2. **Use descriptive names** - `products.read` not `prod_r`
3. **Granular permissions** - `users.delete` separate from `users.update`
4. **Check permissions, not roles** - Code should never check role names
5. **Document permissions** - Include DisplayName and Description
6. **Organize by resource** - `Permissions.Products.*`, `Permissions.Orders.*`
