# Dyvenix Solution Architecture

## Overview

Multi-tenant, multi-application solution with:
- Centralized authentication (Auth service)
- Per-application authorization
- Organization-based data isolation
- Dynamic permission system

## Services

### Auth Service
- OpenIddict OAuth 2.0 server
- ASP.NET Core Identity
- Multi-tenant user management
- Issues JWT tokens with org context and app-specific permissions

### App1 Service
- Business logic (products, orders)
- Validates JWT tokens from Auth service
- Filters data by organization
- Enforces permissions via attributes

### Notifications Service
- Email and SMS sending
- Validates JWT tokens
- Organization context for audit logging

## Authentication Flow

```
1. User → Auth Service (POST /connect/token)
2. Auth validates credentials
3. Auth determines organization and application
4. Auth queries user permissions for org + app
5. Auth generates JWT with permissions
6. User → App1 Service (with JWT in Authorization header)
7. App1 validates JWT
8. App1 extracts organization from token
9. App1 checks permission via [RequirePermission] attribute
10. App1 processes request with org filtering
```

## Authorization Model

### Permissions (Hard-Coded)
- Defined by developers in code
- Examples: "products.read", "users.create"
- Stored in ApplicationPermissions table

### Roles (User-Defined)
- Defined by customers/admins
- Collections of permissions
- Examples: "Manager", "Supervisor"
- Stored in ApplicationRoles table

### Token Structure
```json
{
  "sub": "user-id",
  "email": "user@example.com",
  "organization_id": "org-guid",
  "organization_name": "Acme Corp",
  "is_global_admin": "false",
  "permissions": [
    "products.read",
    "products.create",
    "products.update"
  ],
  "role": "Manager",
  "aud": "app1-api",
  "exp": 1234567890
}
```

## Multi-Tenancy

### Data Isolation
- Every entity has OrganizationId
- All queries filtered by organization
- Users belong to organizations
- Organization ID in JWT token

### Query Pattern
```csharp
var orgId = User.GetOrganizationId();
var products = await _context.Products
    .Where(p => p.OrganizationId == orgId)
    .ToListAsync();
```

## Multi-Application

### Separate Permissions Per App
- App1 has: products.*, orders.*
- Notifications has: notifications.send
- Auth has: users.*, roles.*

### Separate Tokens Per App
- Token for App1 contains App1 permissions
- Token for Notifications contains Notification permissions
- Requested via OAuth scope parameter

## Technology Stack

- .NET 9.0
- ASP.NET Core
- Entity Framework Core
- ASP.NET Core Identity
- OpenIddict 5.8
- .NET Aspire 9.0
- SQL Server
- JWT Bearer Authentication
