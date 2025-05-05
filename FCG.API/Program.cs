using FCG.API.Setup;
using FCG.Application.Modules.Login;
using FCG.Application.Modules.TokenGenerators;
using FCG.Application.Modules.Users;
using FCG.Domain.Modules.Users;
using FCG.Infrastructure;
using System.Text;
using FCG.Infrastructure.Modules.Tokens;
using FCG.Infrastructure.Modules.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

var secret = builder.Configuration["JWT:Secret"];

if (string.IsNullOrEmpty(secret))
{
    throw new InvalidOperationException("O Secret JWT não foi configurado corretamente.");
}

var Issuer = builder.Configuration["JWT:Issuer"];
var Audience = builder.Configuration["JWT:Audience"];;


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//TODO: criar midleware para configuração de swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FCG API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT no campo abaixo. Exemplo: Bearer {seu token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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


builder.Services.RegisterServices();

//TODO: refatorar para Middleware específico??
//Configuração de Conexão com banco
builder.Services.AddDbContext<DbAppContext>(options => options.UseSqlServer(""));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenGenerator>(jw => new JwtTokenGenerator(secret, Issuer, Audience));
builder.Services.AddScoped<ILoginAppServices, LoginAppServices>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
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
                Console.WriteLine("TOKEN VALIDADO");
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Documentação de API para projeto FCG");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].ToString();
    Console.WriteLine("Token recebido: " + token);
    await next();
});

app.UseAuthorization();


//TODO: criar middleware para endpoints??
app.MapGet("/users", async  (IUserAppService userAppService) =>
    {
        return await userAppService.GetAll();
    })
    .WithName("")
    .WithOpenApi()
    .RequireAuthorization("UserOnly", "AdminOnly");

app.MapPost("/Login", async (ILoginAppServices loginAppServices, [FromBody] LoginRequestDto request) =>
{
    var result = await loginAppServices.LoginAppAsync(request);

    if (!result.IsSuccess)
        return Results.Ok(result);

    return Results.Ok(result);

})
.WithOpenApi();

app.MapGet("/admin", () =>
{
    return Results.Ok("Área restrita para Admins");
})
.WithName("GetAdminUsers")
.WithOpenApi()
.RequireAuthorization("UserOnly", "AdminOnly");

app.MapGet("/debug-token", (HttpRequest request) =>
{
    var authHeader = request.Headers["Authorization"].ToString();
    return Results.Ok(new { TokenRecebido = authHeader });
});

app.MapGet("/auth-test", (ClaimsPrincipal user) =>
{
    return Results.Ok(new
    {
        IsAuthenticated = user.Identity?.IsAuthenticated,
        Role = user.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value,
        Claims = user.Claims.Select(c => new { c.Type, c.Value })
    });
}).RequireAuthorization();

app.MapGet("/generate-token", () =>
{
    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Email, "mock@outlook.com"),
        new Claim("userId", Guid.NewGuid().ToString()),
        new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("2RIaHoL7lp5g9jMlDQ1rA1pmoZnZhFM9r6H+Tpq+N9s=")); // Substitua pela sua
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new JwtSecurityToken(
        issuer: "http://localhost:5001",
        audience: "http://localhost:5001",
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(60),
        signingCredentials: creds
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(tokenString);
});


app.Run();