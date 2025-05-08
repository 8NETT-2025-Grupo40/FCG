using FCG.API.Middlewares;
using FCG.Application.Modules.Users;
using FCG.Domain.Modules.Users;
using FCG.Infrastructure.Modules.Users;

namespace FCG.API.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Users
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserAppService, UserAppService>();
        }

        public static void RegisterMiddlewares(this IServiceCollection services)
        {
            services.AddTransient<StructuredLogMiddleware>();
            services.AddTransient<GlobalErrorHandlingMiddleware>();
        }
    }
}
