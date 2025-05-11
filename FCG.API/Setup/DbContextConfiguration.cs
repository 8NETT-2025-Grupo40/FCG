using FCG.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace FCG.API.Setup;

public static class DbContextConfiguration
{
    public static void AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Could find connection string, database will not be configured");
            }

        }, ServiceLifetime.Scoped);
    }
}