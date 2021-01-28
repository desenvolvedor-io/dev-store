using System;
using System.Threading.Tasks;
using Thelema.Core.Messages.Integration;
using Thelema.Pagamentos.API.Models;

namespace Thelema.Pagamentos.API.Services
{
    public interface IPagamentoService
    {
        Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
        Task<ResponseMessage> CapturarPagamento(Guid pedidoId);
        Task<ResponseMessage> CancelarPagamento(Guid pedidoId);
    }
}