using Dyvenix.Auth.Core.Data;
using Dyvenix.Auth.Core.Models;
using Dyvenix.ServiceDefaults.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (Aspire)
builder.AddServiceDefaults();

// Add SQL Server connection
builder.AddSqlServerDbContext<AuthDbContext>("authdb");

// Add Identity
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

// Add OpenIddict
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AuthDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token")
            .SetAuthorizationEndpointUris("/connect/authorize")
            .SetUserinfoEndpointUris("/connect/userinfo");

        options.AllowPasswordFlow()
            .AllowRefreshTokenFlow();

        options.RegisterScopes("openid", "profile", "email", "roles", "app1-api", "notifications-api");

        options.AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

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

// Add authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

// Add authorization with custom handlers
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, AllPermissionsHandler>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

// Add controllers
builder.Services.AddControllers();

// Add API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    await dbContext.Database.MigrateAsync();
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}

// Configure pipeline
app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultEndpoints();

app.Run();
