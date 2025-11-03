-- Sample Queries for Dyvenix Solution

-- View all organizations
SELECT * FROM Organizations;

-- View all applications
SELECT * FROM Applications;

-- View all permissions for an application
SELECT ap.Name, ap.DisplayName, ap.Description, a.Name as ApplicationName
FROM ApplicationPermissions ap
JOIN Applications a ON ap.ApplicationId = a.Id
ORDER BY a.Name, ap.Name;

-- View all roles and their permissions for an application
SELECT 
    ar.Name as RoleName,
    ar.Description,
    ap.Name as PermissionName,
    app.Name as ApplicationName
FROM ApplicationRoles ar
JOIN Applications app ON ar.ApplicationId = app.Id
LEFT JOIN RolePermissions rp ON ar.Id = rp.RoleId
LEFT JOIN ApplicationPermissions ap ON rp.PermissionId = ap.Id
ORDER BY app.Name, ar.Name, ap.Name;

-- View user permissions (via roles) for a specific user
SELECT 
    u.UserName,
    u.Email,
    o.Name as OrganizationName,
    app.Name as ApplicationName,
    ar.Name as RoleName,
    ap.Name as PermissionName
FROM AspNetUsers u
JOIN UserApplicationRoles uar ON u.Id = uar.UserId
JOIN Organizations o ON uar.OrganizationId = o.Id
JOIN Applications app ON uar.ApplicationId = app.Id
JOIN ApplicationRoles ar ON uar.RoleId = ar.Id
JOIN RolePermissions rp ON ar.Id = rp.RoleId
JOIN ApplicationPermissions ap ON rp.PermissionId = ap.Id
WHERE u.Email = 'admin@dyvenix.com'
ORDER BY app.Name, ap.Name;

-- View all users in an organization
SELECT 
    u.UserName,
    u.Email,
    u.FirstName,
    u.LastName,
    ou.IsOwner,
    ou.JoinedAt
FROM AspNetUsers u
JOIN OrganizationUsers ou ON u.Id = ou.UserId
JOIN Organizations o ON ou.OrganizationId = o.Id
WHERE o.Slug = 'acme-corp'
ORDER BY ou.IsOwner DESC, u.UserName;

-- View products by organization (App1 database)
-- SELECT * FROM Products WHERE OrganizationId = '11111111-1111-1111-1111-111111111111';
