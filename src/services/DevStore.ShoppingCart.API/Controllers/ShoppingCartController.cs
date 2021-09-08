using DevStore.ShoppingCart.API.Model;
using DevStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DevStore.WebAPI.Core.User;

namespace DevStore.ShoppingCart.API.Controllers
{
    [Authorize, Route("shopping-cart")]
    public class ShoppingCartController : MainController
    {
        private readonly IAspNetUser _user;
        private readonly Data.ShoppingCartContext _context;

        public ShoppingCartController(IAspNetUser user, Data.ShoppingCartContext context)
        {
            _user = user;
            _context = context;
        }

        [HttpGet("")]
        public async Task<CustomerShoppingCart> GetShoppingCart()
        {
            return await GetShoppingCartClient() ?? new CustomerShoppingCart();
        }

        [HttpPost("")]
        public async Task<IActionResult> AddItem(CartItem item)
        {
            var carrinho = await GetShoppingCartClient();

            if (carrinho == null)
                ManageNewCart(item);
            else
                ManageCart(carrinho, item);

            if (!ValidOperation()) return CustomResponse();

            await Persist();
            return CustomResponse();
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateItem(Guid productId, CartItem item)
        {
            var shoppingCart = await GetShoppingCartClient();
            var shoppingCartItem = await GetValidItem(productId, shoppingCart, item);
            if (shoppingCartItem == null) return CustomResponse();

            shoppingCart.UpdateUnit(shoppingCartItem, item.Quantity);

            ValidateShoppingCart(shoppingCart);
            if (!ValidOperation()) return CustomResponse();

            _context.CartItems.Update(shoppingCartItem);
            _context.CustomerShoppingCart.Update(shoppingCart);

            await Persist();
            return CustomResponse();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var cart = await GetShoppingCartClient();

            var item = await GetValidItem(productId, cart);
            if (item == null) return CustomResponse();

            ValidateShoppingCart(cart);
            if (!ValidOperation()) return CustomResponse();

            cart.RemoveItem(item);

            _context.CartItems.Remove(item);
            _context.CustomerShoppingCart.Update(cart);

            await Persist();
            return CustomResponse();
        }

        [HttpPost]
        [Route("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(Voucher voucher)
        {
            var cart = await GetShoppingCartClient();

            cart.ApplyVoucher(voucher);

            _context.CustomerShoppingCart.Update(cart);

            await Persist();
            return CustomResponse();
        }

        private async Task<CustomerShoppingCart> GetShoppingCartClient()
        {
            return await _context.CustomerShoppingCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }
        private void ManageNewCart(CartItem item)
        {
            var cart = new CustomerShoppingCart(_user.GetUserId());
            cart.AddItem(item);

            ValidateShoppingCart(cart);
            _context.CustomerShoppingCart.Add(cart);
        }
        private void ManageCart(CustomerShoppingCart cart, CartItem item)
        {
            var savedItem = cart.HasItem(item);

            cart.AddItem(item);
            ValidateShoppingCart(cart);

            if (savedItem)
            {
                _context.CartItems.Update(cart.GetProductById(item.ProductId));
            }
            else
            {
                _context.CartItems.Add(item);
            }

            _context.CustomerShoppingCart.Update(cart);
        }
        private async Task<CartItem> GetValidItem(Guid productId, CustomerShoppingCart cart, CartItem item = null)
        {
            if (item != null && productId != item.ProductId)
            {
                AddErrorToStack("Current item is not the same sent item");
                return null;
            }

            if (cart == null)
            {
                AddErrorToStack("Shopping cart not found");
                return null;
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(i => i.ShoppingCartId == cart.Id && i.ProductId == productId);

            if (cartItem == null || !cart.HasItem(cartItem))
            {
                AddErrorToStack("The item is not in cart");
                return null;
            }

            return cartItem;
        }
        private async Task Persist()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddErrorToStack("Error saving data");
        }
        private bool ValidateShoppingCart(CustomerShoppingCart carrinho)
        {
            if (carrinho.IsValid()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(e => AddErrorToStack(e.ErrorMessage));
            return false;
        }
    }
}
