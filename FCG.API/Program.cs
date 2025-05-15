using FCG.API.Endpoints;
using FCG.API.Setup;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.RegisterMiddlewares();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddDbContextConfiguration(builder.Configuration);

builder.ConfigureSerilog();

var app = builder.Build();

app.ConfigureMiddlewares();

// Endpoints
app.MapUserEndpoints();
app.MapAuthenticationEndpoints();

app.MapGet("/admin", () =>
{
    return Results.Ok("Área restrita para Admins");
})
.WithName("GetAdminUsers")
.WithOpenApi()
.RequireAuthorization("AdminOnly");

app.Run();