using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Thelema.Bff.Compras.Extensions;
using Thelema.Bff.Compras.Models;
using Thelema.Core.Communication;

namespace Thelema.Bff.Compras.Services
{
    public interface ICarrinhoService
    {
        Task<CarrinhoDTO> ObterCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher);
    }

    public class CarrinhoService : Service, ICarrinhoService
    {
        private readonly HttpClient _httpClient;

        public CarrinhoService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CarrinhoUrl);
        }

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var Response = await _httpClient.GetAsync("/carrinho/");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<CarrinhoDTO>(Response);
        }

        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoDTO produto)
        {
            var itemContent = ObterConteudo(produto);

            var Response = await _httpClient.PostAsync("/carrinho/", itemContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho)
        {
            var itemContent = ObterConteudo(carrinho);

            var Response = await _httpClient.PutAsync($"/carrinho/{carrinho.ProdutoId}", itemContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }

        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var Response = await _httpClient.DeleteAsync($"/carrinho/{produtoId}");

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }

        public async Task<ResponseResult> AplicarVoucherCarrinho(VoucherDTO voucher)
        {
            var itemContent = ObterConteudo(voucher);

            var Response = await _httpClient.PostAsync("/carrinho/aplicar-voucher/", itemContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }
    }
}