using System;

namespace NSE.Core.Messages.Integration
{
    public class PedidoIniciadoIntegrationEvent : IntegrationEvent
    {
        public Guid ClienteId { get; set; }
        public Guid PedidoId { get; set; }
        public int TipoPagamento { get; set; }
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string MesAnoVencimento { get; set; }
        public string CVV { get; set; }
    }
}