using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NSE.Core.Validation;

namespace NSE.Bff.Compras.Models
{
    public class PedidoDTO
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
        public string VoucherCodigo { get; set; }
        public bool VoucherUtilizado { get; set; }

        public List<ItemCarrinhoDTO> PedidoItems { get; set; }

        #endregion

        #region Endereco

        public EnderecoDTO Endereco { get; set; }

        #endregion

        #region Cartão

        [Required(ErrorMessage = "Informe o número do cartão")]
        [DisplayName("Número do Cartão")]
        public string NumeroCartao { get; set; }

        [Required(ErrorMessage = "Informe o nome do portador do cartão")]
        [DisplayName("Nome do Portador")]
        public string NomeCartao { get; set; }

        [RegularExpression(@"(0[1-9]|1[0-2])\/[0-9]{2}", ErrorMessage = "O vencimento deve estar no padrão MM/AA")]
        [CartaoExpiracao(ErrorMessage = "Cartão Expirado")]
        [Required(ErrorMessage = "Informe o vencimento")]
        [DisplayName("Data de Vencimento MM/AA")]
        public string ExpiracaoCartao { get; set; }

        [Required(ErrorMessage = "Informe o código de segurança")]
        [DisplayName("Código de Segurança")]
        public string CvvCartao { get; set; }

        #endregion
    }
}