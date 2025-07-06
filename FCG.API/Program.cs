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

app.ApplyMigrationsIfConfigured();

app.ConfigureMiddlewares();

// Endpoints
app.MapHealthCheckEndpoints();
app.MapUserEndpoints();
app.MapAuthenticationEndpoints();
app.MapGameEndpoints();

app.Run();