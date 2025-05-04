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
        
        public static IHostApplicationBuilder ConfigureApplicationInsights(this IHostApplicationBuilder builder)
        {
            const string applicationInsightConnectionStringKey = "ApplicationInsightsConnectionString";
            const string applicationCategory = "FIAP Cloud Games";
            
#if RELEASE
            builder.Logging.AddApplicationInsights(
                configureTelemetryConfiguration: (config) => 
                    config.ConnectionString = builder.Configuration.GetConnectionString(applicationInsightConnectionStringKey),
                configureApplicationInsightsLoggerOptions: (_) => { }
            );

            builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>(applicationCategory, LogLevel.Error);
#endif
            
            return builder;
        }
    }
}
