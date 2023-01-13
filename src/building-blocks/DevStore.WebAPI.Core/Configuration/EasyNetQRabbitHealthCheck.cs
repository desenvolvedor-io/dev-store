using EasyNetQ;
using EasyNetQ.ConnectionString;
using HealthChecks.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace DevStore.WebAPI.Core.Configuration;

public static class EasyNetQRabbitHealthCheck
{
    public static IHealthChecksBuilder AddEasyNetQRabbitHealthCheck(this IHealthChecksBuilder healthChecksBuilder, string connectionString)
    {
        var easyNetQConnstringParser = new ConnectionStringParser();
        var connectionConfiguration = easyNetQConnstringParser.Parse(connectionString);
        var connRabbit = CreateConnectionFactory(connectionConfiguration);
        var rabbitHealthCheck = new RabbitMQHealthCheck(connRabbit);
        healthChecksBuilder.Services.AddSingleton(sp => rabbitHealthCheck);

        return healthChecksBuilder.Add(new HealthCheckRegistration(
            "RabbitMQ",
            sp => sp.GetRequiredService<RabbitMQHealthCheck>(),
            default,
            tags: new[] { "infra" },
            default));
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
}