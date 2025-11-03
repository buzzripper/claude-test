using Dyvenix.ServiceDefaults.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults (Aspire)
builder.AddServiceDefaults();

// Get Auth service URL from service discovery
var authUrl = builder.Configuration["services:auth:https:0"] 
    ?? builder.Configuration["services:auth:http:0"] 
    ?? "http://auth";

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authUrl;
        options.Audience = "notifications-api";
        options.RequireHttpsMetadata = false; // For development
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudiences = new[] { "notifications-api" },
            ClockSkew = TimeSpan.FromMinutes(5)
        };
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
