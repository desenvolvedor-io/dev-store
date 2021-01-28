using System;
using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class PedidoViewModel
    {
        #region Pedido

        public int Codigo { get; set; }

        // Autorizado = 1,
        // Pago = 2,
        // Recusado = 3,
        // Entregue = 4,
        // Cancelado = 5
        public int Status { get; set; }
        public DateTime Data { get; set; }
        public decimal ValorTotal { get; set; }

        public decimal Desconto { get; set; }
        public bool VoucherUtilizado { get; set; }

        public List<ItemPedidoViewModel> PedidoItems { get; set; } = new List<ItemPedidoViewModel>();

        #endregion

        #region Item Pedido

        public class ItemPedidoViewModel
        {
            public Guid ProdutoId { get; set; }
            public string Nome { get; set; }
            public int Quantidade { get; set; }
            public decimal Valor { get; set; }
            public string Imagem { get; set; }
        }

        #endregion

        #region Endereco

        public EnderecoViewModel Endereco { get; set; }

        #endregion
    }
}