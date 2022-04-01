using FluentValidation;
using FluentValidation.Results;

namespace DevStore.ShoppingCart.API.Model
{
    public class CustomerShoppingCart
    {
        internal const int MAX_ITEMS = 5;

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Total { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public ValidationResult ValidationResult { get; set; }

        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; }

        public Voucher Voucher { get; set; }

        public CustomerShoppingCart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public CustomerShoppingCart() { }

        public void ApplyVoucher(Voucher voucher)
        {
            Voucher = voucher;
            HasVoucher = true;
            CalculateShoppingCartPrice();
        }

        internal void CalculateShoppingCartPrice()
        {
            Total = Items.Sum(p => p.CalculatePrice());
            CalculateDiscountPrice();
        }

        private void CalculateDiscountPrice()
        {
            if (!HasVoucher) return;

            decimal discount = 0;
            var price = Total;

            if (Voucher.DiscountType == DiscountType.Percentage)
            {
                if (Voucher.Percentage.HasValue)
                {
                    discount = (price * Voucher.Percentage.Value) / 100;
                    price -= discount;
                }
            }
            else
            {
                if (Voucher.Discount.HasValue)
                {
                    discount = Voucher.Discount.Value;
                    price -= discount;
                }
            }

            Total = price < 0 ? 0 : price;
            Discount = discount;
        }

        internal bool HasItem(CartItem item)
        {
            return Items.Any(p => p.ProductId == item.ProductId);
        }

        internal CartItem GetProductById(Guid productId)
        {
            return Items.FirstOrDefault(p => p.ProductId == productId);
        }

        internal void AddItem(CartItem item)
        {
            item.SetShoppingCart(Id);

            if (HasItem(item))
            {
                var itemRef = GetProductById(item.ProductId);
                itemRef.AddUnit(item.Quantity);

                item = itemRef;
                Items.Remove(itemRef);
            }

            Items.Add(item);
            CalculateShoppingCartPrice();
        }

        internal void UpdateItem(CartItem item)
        {
            item.SetShoppingCart(Id);

            var itemExistente = GetProductById(item.ProductId);

            Items.Remove(itemExistente);
            Items.Add(item);

            CalculateShoppingCartPrice();
        }

        internal void UpdateUnit(CartItem item, int unities)
        {
            item.UpdateUnit(unities);
            UpdateItem(item);
        }

        internal void RemoveItem(CartItem item)
        {
            Items.Remove(GetProductById(item.ProductId));
            CalculateShoppingCartPrice();
        }

        internal bool IsValid()
        {
            var errors = Items.SelectMany(i => new CartItem.ShoppingCartItemValidation().Validate(i).Errors).ToList();
            errors.AddRange(new CustomerShoppingCartValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }

        public class CustomerShoppingCartValidation : AbstractValidator<CustomerShoppingCart>
        {
            public CustomerShoppingCartValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Customer not found");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("The shopping cart does not have any items");

                RuleFor(c => c.Total)
                    .GreaterThan(0)
                    .WithMessage("The shopping cart total amount should be greater than 0");
            }
        }
    }
}


