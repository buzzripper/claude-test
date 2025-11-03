-- Multi-Tenant, Multi-Application Schema Reference
-- This shows the complete schema structure (created by EF migrations)

-- Organizations (Tenants)
-- =======================
CREATE TABLE Organizations (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    SubscriptionTier NVARCHAR(50) NOT NULL DEFAULT 'Free'
);

-- Organization Membership
-- =======================
CREATE TABLE OrganizationUsers (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    OrganizationId UNIQUEIDENTIFIER NOT NULL,
    IsOwner BIT NOT NULL DEFAULT 0,
    JoinedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_OrganizationUsers_Users FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrganizationUsers_Organizations FOREIGN KEY (OrganizationId) 
        REFERENCES Organizations(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_OrganizationUsers_UserOrg UNIQUE (UserId, OrganizationId)
);

-- Applications (Your Services)
-- =============================
CREATE TABLE Applications (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    ClientId NVARCHAR(100) NOT NULL UNIQUE,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Application Roles (User-Defined)
-- =================================
CREATE TABLE ApplicationRoles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ApplicationId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    CONSTRAINT FK_ApplicationRoles_Applications FOREIGN KEY (ApplicationId) 
        REFERENCES Applications(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_ApplicationRoles_AppName UNIQUE (ApplicationId, Name)
);

-- Application Permissions (Hard-Coded)
-- =====================================
CREATE TABLE ApplicationPermissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    ApplicationId UNIQUEIDENTIFIER NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    DisplayName NVARCHAR(200),
    Description NVARCHAR(500),
    CONSTRAINT FK_ApplicationPermissions_Applications FOREIGN KEY (ApplicationId) 
        REFERENCES Applications(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_ApplicationPermissions_AppName UNIQUE (ApplicationId, Name)
);

-- Role-Permission Mapping
-- ========================
CREATE TABLE RolePermissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT FK_RolePermissions_Roles FOREIGN KEY (RoleId) 
        REFERENCES ApplicationRoles(Id) ON DELETE CASCADE,
    CONSTRAINT FK_RolePermissions_Permissions FOREIGN KEY (PermissionId) 
        REFERENCES ApplicationPermissions(Id) ON DELETE NO ACTION,
    CONSTRAINT UQ_RolePermissions_RolePerm UNIQUE (RoleId, PermissionId)
);

-- User Role Assignments
-- ======================
CREATE TABLE UserApplicationRoles (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    OrganizationId UNIQUEIDENTIFIER NOT NULL,
    ApplicationId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    GrantedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_UserApplicationRoles_Users FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserApplicationRoles_Organizations FOREIGN KEY (OrganizationId) 
        REFERENCES Organizations(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserApplicationRoles_Applications FOREIGN KEY (ApplicationId) 
        REFERENCES Applications(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserApplicationRoles_Roles FOREIGN KEY (RoleId) 
        REFERENCES ApplicationRoles(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_UserApplicationRoles_All UNIQUE (UserId, OrganizationId, ApplicationId, RoleId)
);

-- User Permission Assignments (Direct)
-- ====================================
CREATE TABLE UserApplicationPermissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    UserId NVARCHAR(450) NOT NULL,
    OrganizationId UNIQUEIDENTIFIER NOT NULL,
    ApplicationId UNIQUEIDENTIFIER NOT NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    GrantedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_UserApplicationPermissions_Users FOREIGN KEY (UserId) 
        REFERENCES AspNetUsers(Id) ON DELETE CASCADE,
    CONSTRAINT FK_UserApplicationPermissions_Organizations FOREIGN KEY (OrganizationId) 
        REFERENCES Organizations(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserApplicationPermissions_Applications FOREIGN KEY (ApplicationId) 
        REFERENCES Applications(Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UserApplicationPermissions_Permissions FOREIGN KEY (PermissionId) 
        REFERENCES ApplicationPermissions(Id) ON DELETE CASCADE,
    CONSTRAINT UQ_UserApplicationPermissions_All UNIQUE (UserId, OrganizationId, ApplicationId, PermissionId)
);

-- Extended AspNetUsers
-- ====================
ALTER TABLE AspNetUsers ADD FirstName NVARCHAR(100);
ALTER TABLE AspNetUsers ADD LastName NVARCHAR(100);
ALTER TABLE AspNetUsers ADD OrganizationId UNIQUEIDENTIFIER;
ALTER TABLE AspNetUsers ADD IsGlobalAdmin BIT NOT NULL DEFAULT 0;

ALTER TABLE AspNetUsers ADD CONSTRAINT FK_AspNetUsers_Organizations 
    FOREIGN KEY (OrganizationId) REFERENCES Organizations(Id) ON DELETE NO ACTION;
