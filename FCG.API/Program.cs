using FCG.API.Setup;
using FCG.Application.Modules.Login;
using FCG.Application.Modules.TokenGenerators;
using FCG.Application.Modules.Users;
using FCG.Domain.Modules.Users;
using FCG.Infrastructure;
using FCG.Infrastructure.Modules.Tokens;
using FCG.Infrastructure.Modules.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();

//TODO: refatorar para Middleware específico??
//Configuração de Conexão com banco
builder.Services.AddDbContext<DbAppContext>(options => options.UseSqlServer(""));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenGenerator>(jw => new JwtTokenGenerator("minhaChaveDeUmaClasseOuVault", "https://localhost:5001", "https://localhost:5001,https://api.localhost:5001"));
builder.Services.AddScoped<ILoginAppServices, LoginAppServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/users", async (IUserAppService userAppService) =>
    {
        return await userAppService.GetAll();
    })
    .WithName("")
    .WithOpenApi();

app.MapPost("/Login", async (ILoginAppServices loginAppServices, [FromBody] LoginRequestDto request) =>
{
    var result = await loginAppServices.LoginAppAsync(request);
    if (!result.IsSuccess)
        return Results.Unauthorized();

    return Results.Ok(new { token = result.Token });
})
.WithOpenApi();

app.Run();