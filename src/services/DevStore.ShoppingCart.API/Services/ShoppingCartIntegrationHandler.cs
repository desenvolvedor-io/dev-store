using DevStore.Core.Messages.Integration;
using DevStore.MessageBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DevStore.ShoppingCart.API.Services
{
    public class ShoppingCartIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public ShoppingCartIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetSubscribers();
            return Task.CompletedTask;
        }

        private void SetSubscribers()
        {
            _bus.SubscribeAsync<OrderDoneIntegrationEvent>("OrderDone", async request =>
                await RemoveShoppingCart(request));
        }

        private async Task RemoveShoppingCart(OrderDoneIntegrationEvent message)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Data.ShoppingCartContext>();

            var carrinho = await context.ShoppingCartClient
                .FirstOrDefaultAsync(c => c.ClientId == message.ClienteId);

            if (carrinho != null)
            {
                context.ShoppingCartClient.Remove(carrinho);
                await context.SaveChangesAsync();
            }
        }
    }
}