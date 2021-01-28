using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Thelema.Bff.Compras.Extensions;
using Thelema.Bff.Compras.Models;
using Thelema.Core.Communication;

namespace Thelema.Bff.Compras.Services
{
    public interface IPedidoService
    {
        Task<ResponseResult> FinalizarPedido(PedidoDTO pedido);
        Task<PedidoDTO> ObterUltimoPedido();
        Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();

        Task<VoucherDTO> ObterVoucherPorCodigo(string codigo);
    }

    public class PedidoService : Service, IPedidoService
    {
        private readonly HttpClient _httpClient;

        public PedidoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.PedidoUrl);
        }

        public async Task<ResponseResult> FinalizarPedido(PedidoDTO pedido)
        {
            var pedidoContent = ObterConteudo(pedido);

            var Response = await _httpClient.PostAsync("/pedido/", pedidoContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }

        public async Task<PedidoDTO> ObterUltimoPedido()
        {
            var Response = await _httpClient.GetAsync("/pedido/ultimo/");

            if (Response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<PedidoDTO>(Response);
        }

        public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId()
        {
            var Response = await _httpClient.GetAsync("/pedido/lista-cliente/");

            if (Response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<IEnumerable<PedidoDTO>>(Response);
        }

        public async Task<VoucherDTO> ObterVoucherPorCodigo(string codigo)
        {
            var Response = await _httpClient.GetAsync($"/voucher/{codigo}/");

            if (Response.StatusCode == HttpStatusCode.NotFound) return null;

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<VoucherDTO>(Response);
        }
    }
}