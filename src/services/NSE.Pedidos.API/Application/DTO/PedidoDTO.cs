using System;
using System.Collections.Generic;
using NSE.Pedidos.Domain.Pedidos;

namespace NSE.Pedidos.API.Application.DTO
{
    public class PedidoDTO
    {
        public Guid Id { get; set; }
        public int Codigo { get; set; }

        public Guid ClienteId { get; set; }
        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }

        public decimal Desconto { get; set; }
        public string VoucherCodigo { get; set; }
        public bool VoucherUtilizado { get; set; }

        public List<PedidoItemDTO> PedidoItems { get; set; }
        public EnderecoDTO Endereco { get; set; }

        public static PedidoDTO ParaPedidoDTO(Pedido pedido)
        {
            var pedidoDTO = new PedidoDTO
            {
                Id = pedido.Id,
                Codigo = pedido.Codigo,
                Status = (int)pedido.PedidoStatus,
                Data = pedido.DataCadastro,
                ValorTotal = pedido.ValorTotal,
                Desconto = pedido.Desconto,
                VoucherUtilizado = pedido.VoucherUtilizado,
                PedidoItems = new List<PedidoItemDTO>(),
                Endereco = new EnderecoDTO()
            };

            foreach (var item in pedido.PedidoItems)
            {
                pedidoDTO.PedidoItems.Add(new PedidoItemDTO
                {
                    Nome = item.ProdutoNome,
                    Imagem = item.ProdutoImagem,
                    Quantidade = item.Quantidade,
                    ProdutoId = item.ProdutoId,
                    Valor = item.ValorUnitario,
                    PedidoId = item.PedidoId
                });
            }

            pedidoDTO.Endereco = new EnderecoDTO
            {
                Logradouro = pedido.Endereco.Logradouro,
                Numero = pedido.Endereco.Numero,
                Complemento = pedido.Endereco.Complemento,
                Bairro = pedido.Endereco.Bairro,
                Cep = pedido.Endereco.Cep,
                Cidade = pedido.Endereco.Cidade,
                Estado = pedido.Endereco.Estado,
            };

            return pedidoDTO;
        }
    }
}