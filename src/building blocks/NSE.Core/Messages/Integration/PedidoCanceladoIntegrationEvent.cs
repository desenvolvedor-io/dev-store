using System;

namespace NSE.Core.Messages.Integration
{
    public class PedidoCanceladoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }

        public PedidoCanceladoIntegrationEvent(Guid clienteId, Guid pedidoId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }
    }
}