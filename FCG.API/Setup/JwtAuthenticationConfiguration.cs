using FCG.Application.Modules.TokenGenerators;
using FCG.Domain.Modules.Users;
using FCG.Infrastructure.Modules.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace FCG.API.Setup;

public static class JwtAuthenticationConfiguration
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secret = configuration["JWT:Secret"];
        var issuer = configuration["JWT:Issuer"];
        var audience = configuration["JWT:Audience"];

        if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            Log.Error("Could find JWT information, authorization and authentication will not be configured");
            throw new InvalidOperationException("Secret JWT is not configured correctly.");
        }

        services.AddScoped<IJwtTokenGenerator>(_ => new JwtTokenGenerator(secret, issuer, audience));

        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(secret)),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("AUTH FAILED: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("TOKEN VALID");
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(PolicyNames.AdminOnly, policy => policy.RequireRole(nameof(UserRole.Admin)));
            options.AddPolicy(PolicyNames.UserOnly, policy => policy.RequireRole(nameof(UserRole.StandardUser)));
        });
    }
}

/// <summary>
/// Nome das Policies, para autorização.
/// </summary>
public static class PolicyNames
{
    public const string AdminOnly = nameof(AdminOnly);
    public const string UserOnly = nameof(UserOnly);
}