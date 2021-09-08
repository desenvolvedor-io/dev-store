using DevStore.ShoppingCart.API.Model;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DevStore.WebAPI.Core.User;

namespace DevStore.ShoppingCart.API.Services.gRPC
{
    [Authorize]
    public class ShoppingCartGrpcService : ShoppingCartOrders.ShoppingCartOrdersBase
    {
        private readonly ILogger<ShoppingCartGrpcService> _logger;

        private readonly IAspNetUser _user;
        private readonly Data.ShoppingCartContext _context;

        public ShoppingCartGrpcService(
            ILogger<ShoppingCartGrpcService> logger,
            IAspNetUser user,
            Data.ShoppingCartContext context)
        {
            _logger = logger;
            _user = user;
            _context = context;
        }

        public override async Task<CustomerShoppingCartClientResponse> GetShoppingCart(GetShoppingCartRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Call GetCart");

            var shoppingCart = await GetShoppingCartClient() ?? new CustomerShoppingCart();

            return MapShoppingCartClientToProtoResponse(shoppingCart);
        }

        private async Task<CustomerShoppingCart> GetShoppingCartClient()
        {
            return await _context.CustomerShoppingCart
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.CustomerId == _user.GetUserId());
        }

        private static CustomerShoppingCartClientResponse MapShoppingCartClientToProtoResponse(CustomerShoppingCart carrinho)
        {
            var carrinhoProto = new CustomerShoppingCartClientResponse
            {
                Id = carrinho.Id.ToString(),
                Customerid = carrinho.CustomerId.ToString(),
                Total = (double)carrinho.Total,
                Discount = (double)carrinho.Discount,
                Hasvoucher = carrinho.HasVoucher,
            };

            if (carrinho.Voucher != null)
            {
                carrinhoProto.Voucher = new VoucherResponse
                {
                    Code = carrinho.Voucher.Code,
                    Percentage = (double?)carrinho.Voucher.Percentage ?? 0,
                    Discount = (double?)carrinho.Voucher.Discount ?? 0,
                    Discounttype = (int)carrinho.Voucher.DiscountType
                };
            }

            foreach (var item in carrinho.Items)
            {
                carrinhoProto.Items.Add(new ShoppingCartItemResponse
                {
                    Id = item.Id.ToString(),
                    Name = item.Name,
                    Image = item.Image,
                    Productid = item.ProductId.ToString(),
                    Quantity = item.Quantity,
                    Price = (double)item.Price
                });
            }

            return carrinhoProto;
        }
    }
}