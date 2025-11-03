using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Dyvenix.Services.Common.Swagger;

public static class SwaggerExtensions
{
    /// <summary>
    /// Adds Swagger with JWT Bearer authentication support
    /// </summary>
    public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services, string title, string version = "v1")
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = title,
                Version = version,
                Description = $"API documentation for {title}"
            });

            // Add JWT Bearer authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Uses Swagger UI in development environment
    /// </summary>
    public static WebApplication UseSwaggerUIDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                options.RoutePrefix = "swagger";
            });
        }

        return app;
    }
}
