using DevStore.ShoppingCart.API.Data;
using DevStore.ShoppingCart.API.Model;
using DevStore.WebAPI.Core.User;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API
{
    public class ShoppingCart
    {
        private readonly ShoppingCartContext _context;
        private readonly IAspNetUser _user;

        private ICollection<string> _errors = new List<string>();

        public ShoppingCart(ShoppingCartContext context, IAspNetUser user)
        {
            _context = context;
            _user = user;
        }

        public async Task<CustomerShoppingCart> GetShoppingCart()
        {
            return await GetShoppingCartClient() ?? new CustomerShoppingCart();
        }

        public async Task<IResult> AddItem(CartItem item)
        {
            var shoppingCart = await GetShoppingCartClient();

            if (shoppingCart == null)
                ManageNewCart(item);
            else
                ManageCart(shoppingCart, item);

            if (_errors.Any()) return CustomResponse();

            await Persist();
            return CustomResponse();
        }

        public async Task<IResult> UpdateItem(Guid productId, CartItem item)
        {
            var shoppingCart = await GetShoppingCartClient();
            var shoppingCartItem = await GetValidItem(productId, shoppingCart, item);
            if (shoppingCartItem == null) return CustomResponse();

            shoppingCart.UpdateUnit(shoppingCartItem, item.Quantity);

            ValidateShoppingCart(shoppingCart);
            if (_errors.Any()) return CustomResponse();

            _context.CartItems.Update(shoppingCartItem);
            _context.CustomerShoppingCart.Update(shoppingCart);

            await Persist();
            return CustomResponse();
        }

        public async Task<IResult> RemoveItem(Guid productId)
        {
            var cart = await GetShoppingCartClient();

            var item = await GetValidItem(productId, cart);
            if (item == null) return CustomResponse();

            ValidateShoppingCart(cart);
            if (_errors.Any()) return CustomResponse();

            cart.RemoveItem(item);

            _context.CartItems.Remove(item);
            _context.CustomerShoppingCart.Update(cart);

            await Persist();
            return CustomResponse();
        }

        public async Task<IResult> ApplyVoucher(Voucher voucher)
        {
            var cart = await GetShoppingCartClient();

            cart.ApplyVoucher(voucher);

            _context.CustomerShoppingCart.Update(cart);

            await Persist();
            return CustomResponse();
        }


        async Task<CustomerShoppingCart> GetShoppingCartClient()
        {
            return await _context.CustomerShoppingCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        void ManageNewCart(CartItem item)
        {
            var cart = new CustomerShoppingCart(_user.GetUserId());
            cart.AddItem(item);

            ValidateShoppingCart(cart);
            _context.CustomerShoppingCart.Add(cart);
        }

        void ManageCart(CustomerShoppingCart cart, CartItem item)
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

        async Task<CartItem> GetValidItem(Guid productId, CustomerShoppingCart cart, CartItem item = null)
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

        async Task Persist()
        {
            var result = await _context.SaveChangesAsync();
            if (result <= 0) AddErrorToStack("Error saving data");
        }

        bool ValidateShoppingCart(CustomerShoppingCart shoppingCart)
        {
            if (shoppingCart.IsValid()) return true;

            shoppingCart.ValidationResult.Errors.ToList().ForEach(e => AddErrorToStack(e.ErrorMessage));
            return false;
        }

        void AddErrorToStack(string error)
        {
            _errors.Add(error);
        }

        IResult CustomResponse(object result = null)
        {
            if (!_errors.Any())
            {
                return Results.Ok(result);
            }

            return Results.BadRequest(Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    { "Messages", _errors.ToArray() }
                }));
        }
    }
}
