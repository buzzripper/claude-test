# Dyvenix Solution - Reorganized Structure

## 🎯 Key Changes in This Version

### ✅ 1. Namespace Updated
**Services.Common namespace changed:**
- **Old:** `Dyvenix.ServiceDefaults`  
- **New:** `Dyvenix.Services.Common`

**Update your using statements:**
```csharp
using Dyvenix.Services.Common.Authorization;
using Dyvenix.Services.Common.Swagger;
```

### ✅ 2. Folder Structure Reorganized
Projects are now organized in service-specific folders:

```
src/
├── AppHost/                      ← Root level
├── Services.Common/              ← Root level  
│   ├── Authorization/
│   └── Swagger/                  ← NEW: Swagger configuration
│
├── Auth/                         ← Folder for Auth service
│   ├── Auth/                     (Dyvenix.Auth.Core)
│   ├── Auth.Host/                (Dyvenix.Auth.Host)
│   └── Auth.Common/              (Dyvenix.Auth.Common)
│
├── App1/                         ← Folder for App1 service
│   ├── App1/                     (Dyvenix.App1.Core)
│   ├── App1.Host/                (Dyvenix.App1.Host)
│   └── App1.Common/              (Dyvenix.App1.Common)
│
└── Notifications/                ← Folder for Notifications service
    ├── Notifications/            (Dyvenix.Notifications.Core)
    ├── Notifications.Host/       (Dyvenix.Notifications.Host)
    └── Notifications.Common/     (Dyvenix.Notifications.Common)
```

**Visual Studio Solution Explorer:**
- AppHost (root)
- Services.Common (root)
- 📁 Auth
  - Auth
  - Auth.Host
  - Auth.Common
- 📁 App1
  - App1
  - App1.Host
  - App1.Common
- 📁 Notifications
  - Notifications
  - Notifications.Host
  - Notifications.Common

### ✅ 3. No Redis
Redis dependencies have been completely removed:
- No Redis in AppHost
- No Redis in Services.Common
- No Redis references anywhere

### ✅ 4. Swagger Configuration Centralized
New `Services.Common/Swagger/SwaggerExtensions.cs`:

```csharp
using Dyvenix.Services.Common.Swagger;

// In Program.cs
builder.Services.AddSwaggerWithAuth("My API");

// In app configuration
app.UseSwaggerUIDevelopment();
```

**Features:**
- Automatic JWT Bearer authentication support
- Swagger UI only in development
- Consistent configuration across all services

### ✅ 5. All Hosts Have Swagger Support
Every Host project now includes:
- Swashbuckle.AspNetCore package (via Services.Common)
- Swagger configuration
- JWT Bearer authentication UI

**Access Swagger:**
- Auth: `http://localhost:{port}/swagger`
- App1: `http://localhost:{port}/swagger`
- Notifications: `http://localhost:{port}/swagger`

## 📁 Complete Project Structure

```
DyvenixSolution/
├── Dyvenix.sln
├── README.md
│
├── sql/              ← Database reference
├── docs/             ← Architecture docs
├── angular/          ← Frontend integration
│
└── src/
    ├── AppHost/
    │   ├── AppHost.csproj
    │   └── Program.cs
    │
    ├── Services.Common/
    │   ├── Services.Common.csproj
    │   ├── Extensions.cs
    │   ├── Authorization/
    │   │   ├── PermissionRequirement.cs
    │   │   ├── AllPermissionsRequirement.cs
    │   │   ├── RequirePermissionAttribute.cs
    │   │   ├── RequireAllPermissionsAttribute.cs
    │   │   ├── PermissionPolicyProvider.cs
    │   │   └── ClaimsPrincipalExtensions.cs
    │   └── Swagger/
    │       └── SwaggerExtensions.cs
    │
    ├── Auth/
    │   ├── Auth/
    │   │   ├── Auth.csproj
    │   │   ├── Models/
    │   │   ├── Data/
    │   │   ├── Services/
    │   │   └── Controllers/
    │   ├── Auth.Host/
    │   │   ├── Auth.Host.csproj
    │   │   ├── Program.cs
    │   │   └── appsettings.json
    │   └── Auth.Common/
    │       ├── Auth.Common.csproj
    │       ├── Constants/Permissions.cs
    │       ├── DTOs/UserDtos.cs
    │       └── Interfaces/IUserManagementService.cs
    │
    ├── App1/
    │   ├── App1/
    │   │   ├── App1.csproj
    │   │   ├── Models/
    │   │   ├── Data/
    │   │   ├── Services/
    │   │   └── Controllers/
    │   ├── App1.Host/
    │   │   ├── App1.Host.csproj
    │   │   ├── Program.cs
    │   │   └── appsettings.json
    │   └── App1.Common/
    │       ├── App1.Common.csproj
    │       ├── Constants/Permissions.cs
    │       ├── DTOs/ProductDtos.cs
    │       └── Interfaces/IProductService.cs
    │
    └── Notifications/
        ├── Notifications/
        │   ├── Notifications.csproj
        │   ├── Services/
        │   └── Controllers/
        ├── Notifications.Host/
        │   ├── Notifications.Host.csproj
        │   ├── Program.cs
        │   └── appsettings.json
        └── Notifications.Common/
            ├── Notifications.Common.csproj
            ├── Constants/Permissions.cs
            ├── DTOs/NotificationDtos.cs
            └── Interfaces/INotificationService.cs
```

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- Visual Studio 2022 (v17.8+) or VS Code
- Docker Desktop

### Run

1. **Start Docker Desktop**

2. **Open solution:**
   ```
   Open Dyvenix.sln in Visual Studio
   ```

3. **Set AppHost as startup project**

4. **Run (F5)**
   - SQL Server starts in Docker
   - Databases created automatically
   - Migrations applied automatically
   - Sample data seeded
   - All services start
   - Aspire Dashboard opens

### Test Endpoints

**Health Checks:**
```http
GET http://localhost:{auth-port}/api/health
GET http://localhost:{app1-port}/api/health
GET http://localhost:{notifications-port}/api/health
```

**Swagger UI:**
```
http://localhost:{auth-port}/swagger
http://localhost:{app1-port}/swagger
http://localhost:{notifications-port}/swagger
```

**Get Token:**
```http
POST http://localhost:{auth-port}/connect/token
Content-Type: application/x-www-form-urlencoded

grant_type=password
&client_id=postman-client
&client_secret=postman-secret-2024
&username=admin@dyvenix.com
&password=Admin123!
&scope=app1-api
```

**Authenticated Endpoint:**
```http
GET http://localhost:{app1-port}/api/health/secure
Authorization: Bearer {token}
```

## 🔧 Using Swagger Extensions

### In Your Program.cs

```csharp
using Dyvenix.Services.Common.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add Swagger with JWT Bearer auth
builder.Services.AddSwaggerWithAuth("My Service API");

// Other services...

var app = builder.Build();

// Use Swagger in development only
app.UseSwaggerUIDevelopment();

// Rest of pipeline...
app.Run();
```

### JWT Authentication in Swagger UI

1. Open Swagger UI (`/swagger`)
2. Click "Authorize" button
3. Enter: `Bearer {your_token}` (include "Bearer " prefix)
4. Click "Authorize"
5. Try authenticated endpoints

## 🔐 Using Authorization

### With Attributes

```csharp
using Dyvenix.Services.Common.Authorization;
using Dyvenix.App1.Common.Constants;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    [HttpGet]
    [RequirePermission(Permissions.Products.Read)]
    public IActionResult GetAll()
    {
        var orgId = User.GetOrganizationId();
        // ...
    }

    [HttpPost]
    [RequireAllPermissions(
        Permissions.Products.Create, 
        Permissions.Products.Update)]
    public IActionResult CreateOrUpdate()
    {
        // User needs BOTH permissions
    }
}
```

### With Extension Methods

```csharp
using Dyvenix.Services.Common.Authorization;

public IActionResult SomeAction()
{
    if (!User.HasPermission(Permissions.Products.Read))
        return Forbid();

    var orgId = User.GetOrganizationId();
    var isAdmin = User.IsGlobalAdmin();
    var permissions = User.GetPermissions();
    
    // ...
}
```

## 📊 Sample Data

**Organizations:**
- Acme Corporation (11111111-1111-1111-1111-111111111111)
- Tech Startup Inc (22222222-2222-2222-2222-222222222222)

**Users (Acme Corp):**
- admin@dyvenix.com / Admin123! - Admin role
- manager@dyvenix.com / Manager123! - Manager role  
- user@dyvenix.com / User123! - User role

**OAuth Client:**
- postman-client / postman-secret-2024

## 🎓 Key Features

### 1. Organized Folder Structure
Services grouped logically in Visual Studio Solution Explorer

### 2. Centralized Swagger Configuration
One place to manage Swagger setup - `Services.Common/Swagger`

### 3. Consistent Namespace
`Dyvenix.Services.Common` everywhere

### 4. No Redis Dependencies
Simpler deployment, fewer moving parts

### 5. Complete Authorization Framework
- Dynamic permission policies
- No policy explosion
- Extension methods for convenience

### 6. Multi-Tenant Architecture
- Organizations, applications, roles, permissions
- Complete data isolation
- Token-based organization context

## 📝 Migration from Previous Version

If you have code from the previous solution:

### 1. Update Namespaces
```csharp
// Old
using Dyvenix.ServiceDefaults.Authorization;

// New
using Dyvenix.Services.Common.Authorization;
```

### 2. Update Project References
Projects now reference `Services.Common` not `ServiceDefaults`

### 3. Update Swagger Configuration
```csharp
// Old
builder.Services.AddSwaggerGen();

// New
builder.Services.AddSwaggerWithAuth("API Title");
app.UseSwaggerUIDevelopment();
```

### 4. Remove Redis References
Delete any Redis-related code

## ✅ What's Complete

- ✅ All 11 projects with correct structure
- ✅ Swagger in Services.Common
- ✅ All Hosts have Swagger support
- ✅ Updated namespace (Dyvenix.Services.Common)
- ✅ Organized folder structure
- ✅ No Redis dependencies
- ✅ Authorization framework
- ✅ Multi-tenant models and DbContext
- ✅ DataSeeder with sample data
- ✅ All Common projects
- ✅ Test controllers

## 🛠️ Next Steps

1. **Test the foundation** - Run and verify everything works
2. **Add your business logic** - Services and controllers
3. **Customize permissions** - Add your app-specific permissions
4. **Build frontend** - Use Angular/React with the auth pattern

---

**Built with .NET 9, Aspire, OpenIddict, Multi-Tenant Architecture**
**Organized structure · Centralized Swagger · No Redis · Clean namespaces**
