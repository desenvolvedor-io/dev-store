using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSE.Core.Data;

namespace NSE.Pagamentos.API.Models
{
    public interface IPagamentoRepository : IRepository<Pagamento>
    {
        void AdicionarPagamento(Pagamento pagamento);
        void AdicionarTransacao(Transacao transacao);
        Task<Pagamento> ObterPagamentoPorPedidoId(Guid pedidoId);
        Task<IEnumerable<Transacao>> ObterTransacaoesPorPedidoId(Guid pedidoId);
    }
}