using DevStore.Core.Utils;
using DevStore.WebAPI.Core.DatabaseFlavor;
using EasyNetQ;
using EasyNetQ.ConnectionString;
using HealthChecks.RabbitMQ;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NetDevPack.Utilities;
using RabbitMQ.Client;
using static DevStore.WebAPI.Core.DatabaseFlavor.ProviderConfiguration;

namespace DevStore.WebAPI.Core.Configuration
{
    public static class GenericHealthCheck
    {

        public static IHealthChecksBuilder AddDefaultHealthCheck(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var (database, connString) = DetectDatabase(configuration);
            var checkBuilder = services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" });

            var rabbitConnStr = configuration.GetMessageQueueConnection("MessageBus");

            // We need to parse EasyNetQ Connection String to RabbitMQ native connectionstring
            // before send it to Health check
            if (rabbitConnStr.IsPresent())
                checkBuilder.AddEasyNetQRabbitHealthCheck(rabbitConnStr);


            return database switch
            {
                DatabaseType.SqlServer => checkBuilder.AddSqlServer(connString, name: "SqlServer", tags: new[] { "infra" }),
                DatabaseType.MySql => checkBuilder.AddMySql(connString, name: "MySql", tags: new[] { "infra" }),
                DatabaseType.Postgre => checkBuilder.AddNpgSql(connString, name: "Postgre", tags: new[] { "infra" }),
                DatabaseType.Sqlite => checkBuilder.AddSqlite(connString, name: "Sqlite", tags: new[] { "infra" }),
                _ => checkBuilder
            };
        }

        private static IConnectionFactory CreateConnectionFactory(ConnectionConfiguration configuration)
        {
            var connectionFactory = new ConnectionFactory
            {
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = false,
                VirtualHost = configuration.VirtualHost,
                UserName = configuration.UserName,
                Password = configuration.Password,
                Port = configuration.Port,
                RequestedHeartbeat = configuration.RequestedHeartbeat,
                ClientProperties = configuration.ClientProperties,
                AuthMechanisms = configuration.AuthMechanisms,
                ClientProvidedName = configuration.Name,
                NetworkRecoveryInterval = configuration.ConnectIntervalAttempt,
                ContinuationTimeout = configuration.Timeout,
                DispatchConsumersAsync = true,
                ConsumerDispatchConcurrency = configuration.PrefetchCount,
            };

            if (configuration.Hosts.Count > 0)
                connectionFactory.HostName = configuration.Hosts[0].Host;

            return connectionFactory;
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
}
