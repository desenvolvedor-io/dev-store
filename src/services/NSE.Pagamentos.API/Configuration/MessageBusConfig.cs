using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Core.Utils;
using NSE.MessageBus;
using NSE.Pagamentos.API.Services;

namespace NSE.Pagamentos.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<PagamentoIntegrationHandler>();
        }
    }
}