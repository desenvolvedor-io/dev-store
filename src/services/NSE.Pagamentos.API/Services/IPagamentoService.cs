using System;
using System.Threading.Tasks;
using NSE.Core.Messages.Integration;
using NSE.Pagamentos.API.Models;

namespace NSE.Pagamentos.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
        Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
    }
}