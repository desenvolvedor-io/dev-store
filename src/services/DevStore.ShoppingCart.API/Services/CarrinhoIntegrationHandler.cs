using System;
using System.Threading;
using System.Threading.Tasks;
using DevStore.Core.Messages.Integration;
using DevStore.MessageBus;
using DevStore.ShoppingCart.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DevStore.ShoppingCart.API.Services
{
    public class CarrinhoIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public CarrinhoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus)
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
            _bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", async request =>
                await ApagarCarrinho(request));
        }

        private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
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