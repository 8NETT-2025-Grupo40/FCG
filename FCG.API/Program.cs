using FCG.API.Setup;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

var secret = builder.Configuration["JWT:Secret"];

if (string.IsNullOrEmpty(secret))
{
    throw new InvalidOperationException("O Secret JWT não foi configurado corretamente.");
}

var Issuer = builder.Configuration["JWT:Issuer"];
var Audience = builder.Configuration["JWT:Audience"];

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//TODO: criar midleware para configuração de swagger


builder.Services.RegisterSwagger();
builder.Services.RegisterServices();
builder.Services.RegisterMiddlewares();

builder.ConfigureSerilog();

//TODO: refatorar para Middleware específico??

if (string.IsNullOrWhiteSpace(Issuer) is false ||
    string.IsNullOrWhiteSpace(Audience) is false)
{
    builder.Services.ConfigureJwt(secret, Issuer, Audience);
}
else
{
    Log.Error("Could find JWT information, authorization and authentication will not be configured");
}

var connectionString = configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString) is false)
{
    builder.Services.ConfigureDatabase(connectionString);
}
else
{
    Log.Error("Could find connection string, database will not be configured");
}

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

app.ConfigureMiddlewares();

app.ConfigureEndpoints();

app.Run();