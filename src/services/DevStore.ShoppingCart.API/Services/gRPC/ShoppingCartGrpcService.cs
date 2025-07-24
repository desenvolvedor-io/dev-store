using DevStore.ShoppingCart.API.Data;
using DevStore.ShoppingCart.API.Model;
using DevStore.WebAPI.Core.User;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DevStore.ShoppingCart.API.Services.gRPC;

[Authorize]
public class ShoppingCartGrpcService : ShoppingCartOrders.ShoppingCartOrdersBase
{
    private readonly ShoppingCartContext _context;
    private readonly ILogger<ShoppingCartGrpcService> _logger;

    private readonly IAspNetUser _user;

    public ShoppingCartGrpcService(
        ILogger<ShoppingCartGrpcService> logger,
        IAspNetUser user,
        ShoppingCartContext context)
    {
        _logger = logger;
        _user = user;
        _context = context;
    }

    public override async Task<CustomerShoppingCartClientResponse> GetShoppingCart(GetShoppingCartRequest request,
        ServerCallContext context)
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

    private static CustomerShoppingCartClientResponse MapShoppingCartClientToProtoResponse(
        CustomerShoppingCart shoppingCart)
    {
        var shoppingCartResponse = new CustomerShoppingCartClientResponse
        {
            Id = shoppingCart.Id.ToString(),
            Customerid = shoppingCart.CustomerId.ToString(),
            Total = (double)shoppingCart.Total,
            Discount = (double)shoppingCart.Discount,
            Hasvoucher = shoppingCart.HasVoucher
        };

        if (shoppingCart.Voucher != null)
            shoppingCartResponse.Voucher = new VoucherResponse
            {
                Code = shoppingCart.Voucher.Code,
                Percentage = (double?)shoppingCart.Voucher.Percentage ?? 0,
                Discount = (double?)shoppingCart.Voucher.Discount ?? 0,
                Discounttype = (int)shoppingCart.Voucher.DiscountType
            };

        foreach (var item in shoppingCart.Items)
            shoppingCartResponse.Items.Add(new ShoppingCartItemResponse
            {
                Id = item.Id.ToString(),
                Name = item.Name,
                Image = item.Image,
                Productid = item.ProductId.ToString(),
                Quantity = item.Quantity,
                Price = (double)item.Price
            });

        return shoppingCartResponse;
    }
}