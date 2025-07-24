using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DevStore.Core.Communication;
using DevStore.WebApp.MVC.Extensions;
using DevStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace DevStore.WebApp.MVC.Services;

public interface ICheckoutBffService
{
    // Shopping cart
    Task<ShoppingCartViewModel> GetShoppingCart();
    Task<int> GetShoppingCartItemsQuantity();
    Task<ResponseResult> AddShoppingCartItem(ShoppingCartItemViewModel shoppingCart);
    Task<ResponseResult> UpdateShoppingCartItem(Guid productId, ShoppingCartItemViewModel shoppingCart);
    Task<ResponseResult> RemoveItemFromShoppingCart(Guid productId);
    Task<ResponseResult> ApplyVoucher(string voucher);

    // Order
    Task<ResponseResult> FinishOrder(TransactionViewModel transaction);
    Task<OrderViewModel> GetLastOrder();
    Task<IEnumerable<OrderViewModel>> GetCustomersById();
    TransactionViewModel MapToOrder(ShoppingCartViewModel shoppingCart, AddressViewModel address);
}

public class CheckoutBffService : Service, ICheckoutBffService
{
    private readonly HttpClient _httpClient;

    public CheckoutBffService(HttpClient httpClient, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(settings.Value.CheckoutBffUrl);
    }


    public async Task<ShoppingCartViewModel> GetShoppingCart()
    {
        var response = await _httpClient.GetAsync("/orders/shopping-cart/");

        ManageResponseErrors(response);

        return await DeserializeResponse<ShoppingCartViewModel>(response);
    }

    public async Task<int> GetShoppingCartItemsQuantity()
    {
        var response = await _httpClient.GetAsync("/orders/shopping-cart/quantity/");

        ManageResponseErrors(response);

        return await DeserializeResponse<int>(response);
    }

    public async Task<ResponseResult> AddShoppingCartItem(ShoppingCartItemViewModel shoppingCart)
    {
        var itemContent = GetContent(shoppingCart);

        var response = await _httpClient.PostAsync("/orders/shopping-cart/items/", itemContent);

        if (!ManageResponseErrors(response)) return await DeserializeResponse<ResponseResult>(response);

        return ReturnOk();
    }

    public async Task<ResponseResult> UpdateShoppingCartItem(Guid productId, ShoppingCartItemViewModel shoppingCartItem)
    {
        var itemContent = GetContent(shoppingCartItem);

        var response = await _httpClient.PutAsync($"/orders/shopping-cart/items/{productId}", itemContent);

        if (!ManageResponseErrors(response)) return await DeserializeResponse<ResponseResult>(response);

        return ReturnOk();
    }

    public async Task<ResponseResult> RemoveItemFromShoppingCart(Guid productId)
    {
        var response = await _httpClient.DeleteAsync($"/orders/shopping-cart/items/{productId}");

        if (!ManageResponseErrors(response)) return await DeserializeResponse<ResponseResult>(response);

        return ReturnOk();
    }

    public async Task<ResponseResult> ApplyVoucher(string voucher)
    {
        var itemContent = GetContent(voucher);

        var response = await _httpClient.PostAsync("/orders/shopping-cart/apply-voucher/", itemContent);

        if (!ManageResponseErrors(response)) return await DeserializeResponse<ResponseResult>(response);

        return ReturnOk();
    }


    public async Task<ResponseResult> FinishOrder(TransactionViewModel transaction)
    {
        var orderContent = GetContent(transaction);

        var response = await _httpClient.PostAsync("/orders", orderContent);

        if (!ManageResponseErrors(response)) return await DeserializeResponse<ResponseResult>(response);

        return ReturnOk();
    }

    public async Task<OrderViewModel> GetLastOrder()
    {
        var response = await _httpClient.GetAsync("/orders/last");

        ManageResponseErrors(response);

        return await DeserializeResponse<OrderViewModel>(response);
    }

    public async Task<IEnumerable<OrderViewModel>> GetCustomersById()
    {
        var response = await _httpClient.GetAsync("/orders/customers");

        ManageResponseErrors(response);

        return await DeserializeResponse<IEnumerable<OrderViewModel>>(response);
    }

    public TransactionViewModel MapToOrder(ShoppingCartViewModel shoppingCart, AddressViewModel address)
    {
        var order = new TransactionViewModel
        {
            Amount = shoppingCart.Total,
            Items = shoppingCart.Items,
            Discount = shoppingCart.Discount,
            HasVoucher = shoppingCart.HasVoucher,
            Voucher = shoppingCart.Voucher?.Voucher
        };

        if (address != null)
            order.Address = new AddressViewModel
            {
                StreetAddress = address.StreetAddress,
                BuildingNumber = address.BuildingNumber,
                Neighborhood = address.Neighborhood,
                ZipCode = address.ZipCode,
                SecondaryAddress = address.SecondaryAddress,
                City = address.City,
                State = address.State
            };

        return order;
    }
}