using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DevStore.WebAPI.Core.Configuration;

public static class MessagingExtensions
{
    public static IHealthChecksBuilder AddMessagingHealthCheck(this IHealthChecksBuilder healthChecksBuilder,
        string connectionString)
    {
        healthChecksBuilder.AddRabbitMQ(async _ =>
            {
                var connectionFactory = new ConnectionFactory
                {
                    Uri = new Uri(connectionString),
                    AutomaticRecoveryEnabled = true
                };
                return await connectionFactory.CreateConnectionAsync();
            },
            tags: ["infra"]);

        return healthChecksBuilder;
    }
}