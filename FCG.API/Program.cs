using FCG.API.Middlewares;
using FCG.API.Setup;
using FCG.Application.Modules.Users;
using FCG.Domain.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();
builder.Services.RegisterMiddlewares();

builder.ConfigureSerilog();
builder.ConfigureApplicationInsights();

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/games", async () => { throw new DomainException("Sample error message"); });

app.MapGet("/users", async (IUserAppService userAppService) => { return await userAppService.GetAll(); })
    .WithName("")
    .WithOpenApi();

app.UseMiddleware<StructuredLogMiddleware>();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.Run();