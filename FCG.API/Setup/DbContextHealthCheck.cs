using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FCG.API.Setup;

public class DbContextHealthCheck<TContext>
    : IHealthCheck where TContext : DbContext
{
    private readonly TContext _dbContext;

    public DbContextHealthCheck(TContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            // Tenta fazer uma query simples, apenas para checar a conexão
            //await this._dbContext.Database.ExecuteSqlAsync($"SELECT 1", cancellationToken);
            return Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (DbException dbEx)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                description: $"Failed to connect in database: {dbEx.Message}",
                exception: dbEx));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(
                description: $"Unexpected error: {ex.Message}",
                exception: ex));
        }
    }
}