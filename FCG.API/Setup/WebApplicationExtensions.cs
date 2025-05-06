using FCG.API.Middlewares;
using FCG.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace FCG.API.Setup;

public static class WebApplicationExtensions
{
    public static WebApplication ConfigureMiddlewares(this WebApplication app)
    {
        // Declara primeiro o Middeware que captura erros da pipeline. Pois se der erro na pipeline, ele deveria proteger
        app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        app.UseMiddleware<StructuredLogMiddleware>();

        app.UseSerilogRequestLogging();
        app.UseHttpsRedirection();
        app.UseSwaggerConfiguration();

        app.UseAuthentication();
        app.UseAuthorization();

        app.Use(async (context, next) =>
        {
            var token = context.Request.Headers["Authorization"].ToString();
            Console.WriteLine("Token recebido: " + token);
            await next();
        });

        return app;
    }

    /// <summary>
    /// Se configurado no modo de execução, aplica as migrations pendentes no banco de dados
    /// </summary>
    public static void ApplyMigrationsIfConfigured(this WebApplication app)
    {
        bool applyMigration =
            // Avalia se pelo launchSettings há a sinalização para aplicar migrations
            app.Configuration.GetValue<bool>("APPLY_MIGRATION")
            // Em produção, nunca deveria rodar migrations automaticamente e sem controle, pois pode gerar perda de dados
            && app.Environment.IsDevelopment();

        if (applyMigration)
        {
            // Cria um escopo de serviço temporário para obter instâncias injetadas, como o DbContext.
            using var scope = app.Services.CreateScope();

            // Recupera o ApplicationDbContext (ou qualquer DbContext que você esteja usando) a partir do container de injeção de dependência.
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Verifica se existem migrations pendentes (ainda não aplicadas ao banco de dados).
            if (dbContext.Database.GetPendingMigrations().Any())
            {
                // Aplica todas as migrations pendentes automaticamente ao banco de dados.
                dbContext.Database.Migrate();
            }
        }
    }
}