using DevStore.Core.Messages.Integration;
using DevStore.ShoppingCart.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API.Services;

public class ShoppingCartIntegrationHandler : IConsumer<OrderDoneIntegrationEvent>
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public ShoppingCartIntegrationHandler(IServiceProvider serviceProvider, IBus bus)
    {
        _serviceProvider = serviceProvider;
        _bus = bus;
    }

    public async Task Consume(ConsumeContext<OrderDoneIntegrationEvent> context)
    {
        await context.RespondAsync(RemoveShoppingCart(context.Message));
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