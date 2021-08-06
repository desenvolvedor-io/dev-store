using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevStore.ShoppingCart.API.Model
{
    public class ShoppingCartClient
    {
        internal const int MAX_QUANTIDADE_ITEM = 5;

        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public decimal Total { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public ValidationResult ValidationResult { get; set; }

        public bool HasVoucher { get; set; }
        public decimal Discount { get; set; }

        public Voucher Voucher { get; set; }

        public ShoppingCartClient(Guid clientId)
        {
            Id = Guid.NewGuid();
            ClientId = clientId;
        }

        public ShoppingCartClient() { }

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

        internal CartItem GetProductById(Guid produtoId)
        {
            return Items.FirstOrDefault(p => p.ProductId == produtoId);
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
            var erros = Items.SelectMany(i => new CartItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();
            erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);
            ValidationResult = new ValidationResult(erros);

            return ValidationResult.IsValid;
        }

        public class CarrinhoClienteValidation : AbstractValidator<ShoppingCartClient>
        {
            public CarrinhoClienteValidation()
            {
                RuleFor(c => c.ClientId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Cliente não reconhecido");

                RuleFor(c => c.Items.Count)
                    .GreaterThan(0)
                    .WithMessage("O carrinho não possui itens");

                RuleFor(c => c.Total)
                    .GreaterThan(0)
                    .WithMessage("O valor total do carrinho precisa ser maior que 0");
            }
        }
    }
}


