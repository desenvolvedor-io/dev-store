using System;

namespace NSE.Core.Messages.Integration
{
    public class PedidoPagoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }

        public PedidoPagoIntegrationEvent(Guid clienteId, Guid pedidoId)
        {
            ClienteId = clienteId;
            PedidoId = pedidoId;
        }
    }
}