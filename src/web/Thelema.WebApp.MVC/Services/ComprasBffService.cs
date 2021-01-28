using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Thelema.Core.Communication;
using Thelema.WebApp.MVC.Extensions;
using Thelema.WebApp.MVC.Models;

namespace Thelema.WebApp.MVC.Services
{
    public interface IComprasBffService
    {
        // Carrinho
        Task<CarrinhoViewModel> ObterCarrinho();
        Task<int> ObterQuantidadeCarrinho();
        Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho);
        Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel carrinho);
        Task<ResponseResult> RemoverItemCarrinho(Guid produtoId);
        Task<ResponseResult> AplicarVoucherCarrinho(string voucher);

        // Pedido
        Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao);
        Task<PedidoViewModel> ObterUltimoPedido();
        Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId();
        PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco);
    }

    public class ComprasBffService : Service, IComprasBffService
    {
        private readonly HttpClient _httpClient;

        public ComprasBffService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.ComprasBffUrl);
        }

        #region Carrinho

        public async Task<CarrinhoViewModel> ObterCarrinho()
        {
            var Response = await _httpClient.GetAsync("/compras/carrinho/");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<CarrinhoViewModel>(Response);
        }
        public async Task<int> ObterQuantidadeCarrinho()
        {
            var Response = await _httpClient.GetAsync("/compras/carrinho-quantidade/");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<int>(Response);
        }
        public async Task<ResponseResult> AdicionarItemCarrinho(ItemCarrinhoViewModel carrinho)
        {
            var itemContent = ObterConteudo(carrinho);

            var Response = await _httpClient.PostAsync("/compras/carrinho/items/", itemContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }
        public async Task<ResponseResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel item)
        {
            var itemContent = ObterConteudo(item);

            var Response = await _httpClient.PutAsync($"/compras/carrinho/items/{produtoId}", itemContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }
        public async Task<ResponseResult> RemoverItemCarrinho(Guid produtoId)
        {
            var Response = await _httpClient.DeleteAsync($"/compras/carrinho/items/{produtoId}");

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }
        public async Task<ResponseResult> AplicarVoucherCarrinho(string voucher)
        {
            var itemContent = ObterConteudo(voucher);

            var Response = await _httpClient.PostAsync("/compras/carrinho/aplicar-voucher/", itemContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }

        #endregion

        #region Pedido

        public async Task<ResponseResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
        {
            var pedidoContent = ObterConteudo(pedidoTransacao);

            var Response = await _httpClient.PostAsync("/compras/pedido/", pedidoContent);

            if (!TratarErrosResponse(Response)) return await DeserializarObjetoResponse<ResponseResult>(Response);

            return RetornoOk();
        }

        public async Task<PedidoViewModel> ObterUltimoPedido()
        {
            var Response = await _httpClient.GetAsync("/compras/pedido/ultimo/");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<PedidoViewModel>(Response);
        }

        public async Task<IEnumerable<PedidoViewModel>> ObterListaPorClienteId()
        {
            var Response = await _httpClient.GetAsync("/compras/pedido/lista-cliente/");

            TratarErrosResponse(Response);

            return await DeserializarObjetoResponse<IEnumerable<PedidoViewModel>>(Response);
        }

        public PedidoTransacaoViewModel MapearParaPedido(CarrinhoViewModel carrinho, EnderecoViewModel endereco)
        {
            var pedido = new PedidoTransacaoViewModel
            {
                ValorTotal = carrinho.ValorTotal,
                Itens = carrinho.Itens,
                Desconto = carrinho.Desconto,
                VoucherUtilizado = carrinho.VoucherUtilizado,
                VoucherCodigo = carrinho.Voucher?.Codigo
            };

            if (endereco != null)
            {
                pedido.Endereco = new EnderecoViewModel
                {
                    Logradouro = endereco.Logradouro,
                    Numero = endereco.Numero,
                    Bairro = endereco.Bairro,
                    Cep = endereco.Cep,
                    Complemento = endereco.Complemento,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado
                };
            }

            return pedido;
        }

        #endregion
    }
}