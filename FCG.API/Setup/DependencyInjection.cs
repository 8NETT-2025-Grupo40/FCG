using FCG.Application.Modules.Users;
using FCG.Domain.Common;
using FCG.Domain.Modules.Users;
using FCG.Infrastructure;
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

            // Common
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
