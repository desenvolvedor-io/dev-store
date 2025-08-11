using DevStore.Infra.Core.DatabaseFlavor;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static DevStore.Infra.Core.DatabaseFlavor.ProviderConfiguration;

namespace DevStore.Infra.Core.Configuration;

public static class GenericHealthCheck
{
    public static IHealthChecksBuilder AddDefaultHealthCheck(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var checkBuilder = services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), new[] { "api" });

        var (database, connString) = DetectDatabase(configuration);
        return database switch
        {
            DatabaseType.SqlServer => checkBuilder.AddSqlServer(connString, name: "SqlServer", tags: new[] { "infra" }),
            DatabaseType.MySql => checkBuilder.AddMySql(connString, name: "MySql", tags: new[] { "infra" }),
            DatabaseType.Postgre => checkBuilder.AddNpgSql(connString, name: "Postgre", tags: new[] { "infra" }),
            DatabaseType.Sqlite => checkBuilder.AddSqlite(connString, name: "Sqlite", tags: new[] { "infra" }),
            _ => checkBuilder
        };
    }

    public static IApplicationBuilder UseDefaultHealthcheck(this IApplicationBuilder app)
    {
        app.UseHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("api"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHealthChecks("/healthz-infra", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("infra"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}