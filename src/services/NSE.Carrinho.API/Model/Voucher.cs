namespace NSE.Carrinho.API.Model
{
    public class Voucher
    {
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string Codigo { get; set; }
        public TipoDescontoVoucher TipoDesconto { get; set; }
    }

    public enum TipoDescontoVoucher
    {
        Porcentagem = 0,
        Valor = 1
    }
}