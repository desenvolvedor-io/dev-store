using System;
using System.Collections.Generic;

namespace NSE.WebApp.MVC.Models
{
    public class CarrinhoViewModel
    {
        public decimal ValorTotal { get; set; }
        public VoucherViewModel Voucher { get; set; }
        public bool VoucherUtilizado { get; set; }
        public decimal Desconto { get; set; }
        public List<ItemCarrinhoViewModel> Itens { get; set; } = new List<ItemCarrinhoViewModel>();
    }

    public class ItemCarrinhoViewModel
    {
        public Guid ProdutoId { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string Imagem { get; set; }
    }
}