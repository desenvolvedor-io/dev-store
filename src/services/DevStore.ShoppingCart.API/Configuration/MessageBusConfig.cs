using System.Reflection;
using DevStore.MessageBus;

namespace DevStore.ShoppingCart.API.Configuration;

public static class MessageBusConfig
{
    public static void AddMessageBusConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMessageBus(configuration, Assembly.GetAssembly(typeof(Program)));
    }
}