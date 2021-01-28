using System;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Application.DTO
{
    public class PedidoItemDTO
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }
        public int Quantidade { get; set; }

        public static PedidoItem ParaPedidoItem(PedidoItemDTO pedidoItemDTO)
        {
            return new PedidoItem(pedidoItemDTO.ProdutoId, pedidoItemDTO.Nome, pedidoItemDTO.Quantidade,
                pedidoItemDTO.Valor, pedidoItemDTO.Imagem);
        }
    }
}