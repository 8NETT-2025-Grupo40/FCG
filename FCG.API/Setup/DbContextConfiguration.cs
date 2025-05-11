using FCG.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FCG.API.Setup;

public static class DbContextConfiguration
{
    public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString) is false)
            {
                options.UseSqlServer(connectionString);
            }
            else
            {
                Log.Error("Could find connection string, database will not be configured");
            }

        }, ServiceLifetime.Scoped);
    }
}