-- Reference SQL Script for Dyvenix Solution
-- Databases are created automatically by Aspire and EF Core migrations

-- Auth Database Schema
-- =====================
-- The Auth database will contain:
-- 1. ASP.NET Core Identity tables (AspNetUsers, AspNetRoles, etc.)
-- 2. OpenIddict tables (OpenIddictApplications, OpenIddictTokens, etc.)
-- 3. Multi-tenant tables (Organizations, OrganizationUsers)
-- 4. Multi-application tables (Applications, ApplicationRoles, ApplicationPermissions)
-- 5. User assignment tables (UserApplicationRoles, UserApplicationPermissions)

-- Run migrations:
-- cd src/Auth.Host
-- dotnet ef database update

-- App1 Database Schema
-- =====================
-- The App1 database will contain:
-- 1. Products table
-- 2. Orders table (if implemented)
-- All with OrganizationId for multi-tenant isolation

-- Run migrations:
-- cd src/App1.Host
-- dotnet ef database update
