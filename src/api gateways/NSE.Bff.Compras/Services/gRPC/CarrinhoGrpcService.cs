using System;
using System.Threading.Tasks;
using NSE.Bff.Compras.Models;
using NSE.Carrinho.API.Services.gRPC;

namespace NSE.Bff.Compras.Services.gRPC
{
    public interface ICarrinhoGrpcService
    {
        Task<CarrinhoDTO> ObterCarrinho();
    }

    public class CarrinhoGrpcService : ICarrinhoGrpcService
    {
        private readonly CarrinhoCompras.CarrinhoComprasClient _carrinhoComprasClient;

        public CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoComprasClient)
        {
            _carrinhoComprasClient = carrinhoComprasClient;
        }

        public async Task<CarrinhoDTO> ObterCarrinho()
        {
            var response = await _carrinhoComprasClient.ObterCarrinhoAsync(new ObterCarrinhoRequest());
            return MapCarrinhoClienteProtoResponseToDTO(response);
        }

        private static CarrinhoDTO MapCarrinhoClienteProtoResponseToDTO(CarrinhoClienteResponse carrinhoResponse)
        {
            var carrinhoDTO = new CarrinhoDTO
            {
                ValorTotal = (decimal)carrinhoResponse.Valortotal,
                Desconto = (decimal)carrinhoResponse.Desconto,
                VoucherUtilizado = carrinhoResponse.Voucherutilizado
            };

            if (carrinhoResponse.Voucher != null)
            {
                carrinhoDTO.Voucher = new VoucherDTO
                {
                    Codigo = carrinhoResponse.Voucher.Codigo,
                    Percentual = (decimal?)carrinhoResponse.Voucher.Percentual,
                    ValorDesconto = (decimal?)carrinhoResponse.Voucher.Valordesconto,
                    TipoDesconto = carrinhoResponse.Voucher.Tipodesconto
                };
            }

            foreach (var item in carrinhoResponse.Itens)
            {
                carrinhoDTO.Itens.Add(new ItemCarrinhoDTO
                {
                    Nome = item.Nome,
                    Imagem = item.Imagem,
                    ProdutoId = Guid.Parse(item.Produtoid),
                    Quantidade = item.Quantidade,
                    Valor = (decimal)item.Valor
                });
            }

            return carrinhoDTO;
        }
    }
}