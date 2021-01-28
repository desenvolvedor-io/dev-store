using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thelema.Core.Utils;
using Thelema.MessageBus;
using Thelema.Pagamentos.API.Services;

namespace Thelema.Pagamentos.API.Configuration
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