using FCG.API.Setup;
using FCG.Application.Modules.Users;

var builder = WebApplication.CreateBuilder(args);

// AddAsync services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// TODO: Criar classes estáticas para separar os endpoints
app.MapGet("/users", async (IUserAppService userAppService) =>
    {
        return await userAppService.GetAll();
    })
    .WithName("")
    .WithOpenApi();

app.MapPost("/users", async (CreateUserRequest request, IUserAppService service, CancellationToken cancellationToken) =>
    {
        Guid id = await service.CreateUserAsync(request, cancellationToken);
        return Results.CreatedAtRoute("GetUserById", new { id }, new { id });
    });

app.MapGet("/users/{id:guid}", async (Guid id, IUserAppService service, CancellationToken cancellationToken) =>
    {
        UserResponse? user = await service.GetByIdAsync(id, cancellationToken);
        return user is null ? Results.NotFound() : Results.Ok(user);
    })
    .WithName("GetUserById");

app.Run();