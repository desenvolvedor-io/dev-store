using DevStore.Core.Utils;
using DevStore.MessageBus;
using DevStore.ShoppingCart.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevStore.ShoppingCart.API.Configuration
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