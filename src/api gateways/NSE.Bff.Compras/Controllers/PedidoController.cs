using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.Bff.Compras.Models;
using NSE.Bff.Compras.Services;
using NSE.WebAPI.Core.Controllers;

namespace NSE.Bff.Compras.Controllers
{
    [Authorize]
    public class PedidoController : MainController
    {
        private readonly ICatalogoService _catalogoService;
        private readonly ICarrinhoService _carrinhoService;
        private readonly IPedidoService _pedidoService;
        private readonly IClienteService _clienteService;

        public PedidoController(
            ICatalogoService catalogoService,
            ICarrinhoService carrinhoService,
            IPedidoService pedidoService,
            IClienteService clienteService)
        {
            _catalogoService = catalogoService;
            _carrinhoService = carrinhoService;
            _pedidoService = pedidoService;
            _clienteService = clienteService;
        }

        [HttpPost]
        [Route("compras/pedido")]
        public async Task<IActionResult> AdicionarPedido(PedidoDTO pedido)
        {
            var carrinho = await _carrinhoService.ObterCarrinho();
            var produtos = await _catalogoService.ObterItens(carrinho.Itens.Select(p => p.ProdutoId));
            var endereco = await _clienteService.ObterEndereco();

            if (!await ValidarCarrinhoProdutos(carrinho, produtos)) return CustomResponse();

            PopularDadosPedido(carrinho, endereco, pedido);

            return CustomResponse(await _pedidoService.FinalizarPedido(pedido));
        }

        [HttpGet("compras/pedido/ultimo")]
        public async Task<IActionResult> UltimoPedido()
        {
            var pedido = await _pedidoService.ObterUltimoPedido();
            if (pedido is null)
            {
                AdicionarErroProcessamento("Pedido não encontrado!");
                return CustomResponse();
            }

            return CustomResponse(pedido);
        }

        [HttpGet("compras/pedido/lista-cliente")]
        public async Task<IActionResult> ListaPorCliente()
        {
            var pedidos = await _pedidoService.ObterListaPorClienteId();

            return pedidos == null ? NotFound() : CustomResponse(pedidos);
        }

        private async Task<bool> ValidarCarrinhoProdutos(CarrinhoDTO carrinho, IEnumerable<ItemProdutoDTO> produtos)
        {
            if (carrinho.Itens.Count != produtos.Count())
            {
                var itensIndisponiveis = carrinho.Itens.Select(c => c.ProdutoId).Except(produtos.Select(p => p.Id)).ToList();

                foreach (var itemId in itensIndisponiveis)
                {
                    var itemCarrinho = carrinho.Itens.FirstOrDefault(c => c.ProdutoId == itemId);
                    AdicionarErroProcessamento($"O item {itemCarrinho.Nome} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
                }

                return false;
            }

            foreach (var itemCarrinho in carrinho.Itens)
            {
                var produtoCatalogo = produtos.FirstOrDefault(p => p.Id == itemCarrinho.ProdutoId);

                if (produtoCatalogo.Valor != itemCarrinho.Valor)
                {
                    var msgErro = $"O produto {itemCarrinho.Nome} mudou de valor (de: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCarrinho.Valor)} para: " +
                                  $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", produtoCatalogo.Valor)}) desde que foi adicionado ao carrinho.";

                    AdicionarErroProcessamento(msgErro);

                    var responseRemover = await _carrinhoService.RemoverItemCarrinho(itemCarrinho.ProdutoId);
                    if (ResponsePossuiErros(responseRemover))
                    {
                        AdicionarErroProcessamento($"Não foi possível remover automaticamente o produto {itemCarrinho.Nome} do seu carrinho, _" +
                                                   "remova e adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    itemCarrinho.Valor = produtoCatalogo.Valor;
                    var responseAdicionar = await _carrinhoService.AdicionarItemCarrinho(itemCarrinho);

                    if (ResponsePossuiErros(responseAdicionar))
                    {
                        AdicionarErroProcessamento($"Não foi possível atualizar automaticamente o produto {itemCarrinho.Nome} do seu carrinho, _" +
                                                   "adicione novamente caso ainda deseje comprar este item");
                        return false;
                    }

                    LimparErrosProcessamento();
                    AdicionarErroProcessamento(msgErro + " Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                    return false;
                }
            }

            return true;
        }
        
        private void PopularDadosPedido(CarrinhoDTO carrinho, EnderecoDTO endereco, PedidoDTO pedido)
        {
            pedido.VoucherCodigo = carrinho.Voucher?.Codigo;
            pedido.VoucherUtilizado = carrinho.VoucherUtilizado;
            pedido.ValorTotal = carrinho.ValorTotal;
            pedido.Desconto = carrinho.Desconto;
            pedido.PedidoItems = carrinho.Itens;

            pedido.Endereco = endereco;
        }
    }
}
