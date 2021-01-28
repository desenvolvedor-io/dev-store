namespace NSE.Bff.Compras.Models
{
    public class VoucherDTO
    {
        public decimal? Percentual { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string Codigo { get; set; }
        public int TipoDesconto { get; set; }
    }
}