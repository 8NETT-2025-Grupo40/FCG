using FCG.API.Setup;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
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
app.ConfigureEndpoints();

app.Run();