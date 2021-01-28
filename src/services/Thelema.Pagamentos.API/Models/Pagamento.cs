using System;
using System.Collections.Generic;
using Thelema.Core.DomainObjects;

namespace Thelema.Pagamentos.API.Models
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Pagamento()
        {
            Transacoes = new List<Transacao>();
        }

        public Guid PedidoId { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public decimal Valor { get; set; }

        public CartaoCredito CartaoCredito { get; set; }

        // EF Relation
        public ICollection<Transacao> Transacoes { get; set; }

        public void AdicionarTransacao(Transacao transacao)
        {
            Transacoes.Add(transacao);
        }
    }
}