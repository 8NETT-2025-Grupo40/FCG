using Serilog;

namespace FCG.API.Setup;

public static class WebApplicationBuilderExtensions
{
    public static IHostApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });
            
        return builder;
    }
        
    public static IHostApplicationBuilder ConfigureApplicationInsights(this WebApplicationBuilder builder)
    {
            
#if RELEASE
            const string applicationInsightConnectionStringKey = "ApplicationInsightsConnectionString";
            const string applicationCategory = "FIAP Cloud Games";
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