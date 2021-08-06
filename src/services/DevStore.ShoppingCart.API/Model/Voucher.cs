namespace DevStore.ShoppingCart.API.Model
{
    public class Voucher
    {
        public decimal? Percentage { get; set; }
        public decimal? Discount { get; set; }
        public string Code { get; set; }
        public DiscountType DiscountType { get; set; }
    }

    public enum DiscountType
    {
        Percentage = 0,
        Value = 1
    }
}