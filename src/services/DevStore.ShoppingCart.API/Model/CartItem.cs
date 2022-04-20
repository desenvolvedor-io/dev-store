using FluentValidation;
using System.Text.Json.Serialization;

namespace DevStore.ShoppingCart.API.Model
{
    public class CartItem
    {
        public CartItem()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }

        public Guid ShoppingCartId { get; set; }

        [JsonIgnore]
        public CustomerShoppingCart CustomerShoppingCart { get; set; }

        internal void SetShoppingCart(Guid carrinhoId)
        {
            ShoppingCartId = carrinhoId;
        }

        internal decimal CalculatePrice()
        {
            return Quantity * Price;
        }

        internal void AddUnit(int quantity)
        {
            Quantity += quantity;
        }

        internal void UpdateUnit(int quantity)
        {
            Quantity = quantity;
        }

        internal bool IsValid()
        {
            return new ShoppingCartItemValidation().Validate(this).IsValid;
        }

        public class ShoppingCartItemValidation : AbstractValidator<CartItem>
        {
            public ShoppingCartItemValidation()
            {
                RuleFor(c => c.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid product Id");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("Procut name must be set");

                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage(item => $"The minimal quantity for {item.Name} is 1");

                RuleFor(c => c.Quantity)
                    .LessThanOrEqualTo(CustomerShoppingCart.MAX_ITEMS)
                    .WithMessage(item => $"The max quantity for {item.Name} is {CustomerShoppingCart.MAX_ITEMS}");

                RuleFor(c => c.Price)
                    .GreaterThan(0)
                    .WithMessage(item => $"The price of {item.Name} must be greater than 0");
            }
        }
    }
}