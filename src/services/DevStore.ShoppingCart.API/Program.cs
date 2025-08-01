using DevStore.ShoppingCart.API;
using DevStore.ShoppingCart.API.Configuration;
using DevStore.ShoppingCart.API.Model;
using DevStore.WebAPI.Core.Configuration;
using DevStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogger(builder.Configuration);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.RegisterServices();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();


app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

MapActions(app);

DbMigrationHelpers.EnsureSeedData(app).Wait();

app.Run();

return;

// ReSharper disable once VariableHidesOuterVariable
void MapActions(WebApplication app)
{
    app.MapGet("/shopping-cart", [Authorize] async (
            ShoppingCart cart) => await cart.GetShoppingCart())
        .WithName("GetShoppingCart")
        .WithTags("ShoppingCart");

    app.MapPost("/shopping-cart", [Authorize] async (
            ShoppingCart cart,
            CartItem item) => await cart.AddItem(item))
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("AddItem")
        .WithTags("ShoppingCart");

    app.MapPut("/shopping-cart/{productId}", [Authorize] async (
            ShoppingCart cart,
            Guid productId,
            CartItem item) => await cart.UpdateItem(productId, item))
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("UpdateItem")
        .WithTags("ShoppingCart");

    app.MapDelete("/shopping-cart/{productId}", [Authorize] async (
            ShoppingCart cart,
            Guid productId) => await cart.RemoveItem(productId))
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("RemoveItem")
        .WithTags("ShoppingCart");

    app.MapPost("/shopping-cart/apply-voucher", [Authorize] async (
            ShoppingCart cart,
            Voucher voucher) => await cart.ApplyVoucher(voucher))
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("ApplyVoucher")
        .WithTags("ShoppingCart");
}