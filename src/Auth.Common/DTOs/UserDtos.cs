namespace Dyvenix.Auth.Common.DTOs;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public bool IsGlobalAdmin { get; set; }
    public List<string> Roles { get; set; } = new();
    public List<string> Permissions { get; set; } = new();
}

public class CreateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid OrganizationId { get; set; }
}

public class AssignRoleRequest
{
    public string UserId { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid RoleId { get; set; }
}

public class AssignPermissionRequest
{
    public string UserId { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid PermissionId { get; set; }
}

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}

public class OrganizationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string SubscriptionTier { get; set; } = string.Empty;
}

public class ApplicationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
