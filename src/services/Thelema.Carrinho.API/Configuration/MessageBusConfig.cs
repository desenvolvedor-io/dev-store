using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Thelema.Carrinho.API.Services;
using Thelema.Core.Utils;
using Thelema.MessageBus;

namespace Thelema.Carrinho.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<CarrinhoIntegrationHandler>();
        }
    }
}