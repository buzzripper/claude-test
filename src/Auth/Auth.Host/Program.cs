using Dyvenix.Auth.Core.Data;
using Dyvenix.Auth.Core.Models;
using Dyvenix.Services.Common.Authorization;
using Dyvenix.Services.Common.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSqlServerDbContext<AuthDbContext>("authdb");

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AuthDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore().UseDbContext<AuthDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token")
            .SetAuthorizationEndpointUris("/connect/authorize")
            .SetUserinfoEndpointUris("/connect/userinfo");
        options.AllowPasswordFlow().AllowRefreshTokenFlow();
        options.RegisterScopes("openid", "profile", "email", "roles", "app1-api", "notifications-api");
        options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
        options.UseAspNetCore()
            .EnableTokenEndpointPassthrough()
            .EnableAuthorizationEndpointPassthrough()
            .EnableUserinfoEndpointPassthrough();
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AllPermissionsHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

builder.Services.AddControllers();
builder.Services.AddSwaggerWithAuth("Auth API");

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await dbContext.Database.MigrateAsync();
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}

app.UseSwaggerUIDevelopment();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
