using DevStore.Core.Messages.Integration;
using DevStore.ShoppingCart.API.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API.Services;

public class ShoppingCartIntegrationHandler : IConsumer<OrderDoneIntegrationEvent>
{
    private readonly IServiceProvider _serviceProvider;

    public ShoppingCartIntegrationHandler(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<OrderDoneIntegrationEvent> context)
    {
        await RemoveShoppingCart(context.Message);
        await context.RespondAsync(new object());
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