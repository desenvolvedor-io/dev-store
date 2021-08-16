using DevStore.ShoppingCart.API.Model;
using DevStore.WebAPI.Core.Controllers;
using DevStore.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ShoppingCartClient> GetShoppingCart()
        {
            return await GetShoppingCartClient() ?? new ShoppingCartClient();
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
            var carrinho = await GetShoppingCartClient();
            var itemCarrinho = await GetValidItem(productId, carrinho, item);
            if (itemCarrinho == null) return CustomResponse();

            carrinho.UpdateUnit(itemCarrinho, item.Quantity);

            ValidateShoppingCart(carrinho);
            if (!ValidOperation()) return CustomResponse();

            _context.CartItems.Update(itemCarrinho);
            _context.ShoppingCartClient.Update(carrinho);

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
            _context.ShoppingCartClient.Update(cart);

            await Persist();
            return CustomResponse();
        }

        [HttpPost]
        [Route("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(Voucher voucher)
        {
            var cart = await GetShoppingCartClient();

            cart.ApplyVoucher(voucher);

            _context.ShoppingCartClient.Update(cart);

            await Persist();
            return CustomResponse();
        }

        private async Task<ShoppingCartClient> GetShoppingCartClient()
        {
            return await _context.ShoppingCartClient
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.ClientId == _user.GetUserId());
        }
        private void ManageNewCart(CartItem item)
        {
            var cart = new ShoppingCartClient(_user.GetUserId());
            cart.AddItem(item);

            ValidateShoppingCart(cart);
            _context.ShoppingCartClient.Add(cart);
        }
        private void ManageCart(ShoppingCartClient cart, CartItem item)
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

            _context.ShoppingCartClient.Update(cart);
        }
        private async Task<CartItem> GetValidItem(Guid productId, ShoppingCartClient cart, CartItem item = null)
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
        private bool ValidateShoppingCart(ShoppingCartClient carrinho)
        {
            if (carrinho.IsValid()) return true;

            carrinho.ValidationResult.Errors.ToList().ForEach(e => AddErrorToStack(e.ErrorMessage));
            return false;
        }
    }
}
