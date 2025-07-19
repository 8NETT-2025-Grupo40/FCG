using FCG.API.Middlewares;
using FCG.Application.Login;
using FCG.Application.Users;
using FCG.Domain.Common;
using FCG.Domain.Users.Repositories;
using FCG.Infrastructure;
using FCG.Infrastructure.Repositories;

namespace FCG.API.Setup;

public static class DependencyInjection
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // Users
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAppService, UserAppService>();

        // Autenticação
        services.AddScoped<ILoginAppServices, LoginAppServices>();

        // Common
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void RegisterMiddlewares(this IServiceCollection services)
    {
        services.AddTransient<StructuredLogMiddleware>();
        services.AddTransient<GlobalErrorHandlingMiddleware>();
    }
}