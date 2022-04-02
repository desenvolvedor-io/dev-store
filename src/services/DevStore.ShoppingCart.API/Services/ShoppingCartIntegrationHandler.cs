using DevStore.Core.Messages.Integration;
using DevStore.MessageBus;
using DevStore.ShoppingCart.API.Data;
using Microsoft.EntityFrameworkCore;

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
            var context = scope.ServiceProvider.GetRequiredService<ShoppingCartContext>();

            var shoppingCart = await context.CustomerShoppingCart
                .FirstOrDefaultAsync(c => c.CustomerId == message.CustomerId);

            if (shoppingCart != null)
            {
                context.CustomerShoppingCart.Remove(shoppingCart);
                await context.SaveChangesAsync();
            }
        }
    }
}