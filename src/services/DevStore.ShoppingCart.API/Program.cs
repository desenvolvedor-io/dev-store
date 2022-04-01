using DevStore.ShoppingCart.API.Configuration;
using DevStore.ShoppingCart.API.Data;
using DevStore.ShoppingCart.API.Model;
using DevStore.WebAPI.Core.Identity;
using DevStore.WebAPI.Core.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

#region Builder Configuration

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Configuration.AddUserSecrets<Program>();

#endregion

#region Configure Services

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddSwaggerConfiguration();

builder.Services.RegisterServices();

builder.Services.AddMessageBusConfiguration(builder.Configuration);

var app = builder.Build();

#endregion

#region Configure Pipeline

var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<ShoppingCartContext>();
var user = app.Services.CreateScope().ServiceProvider.GetRequiredService<IAspNetUser>();
var Errors = new List<string>();

app.UseSwaggerConfiguration();

app.UseApiConfiguration(app.Environment);

MapActions(app);

DbMigrationHelpers.EnsureSeedData(app).Wait();

app.Run();

#endregion

#region Actions

void MapActions(WebApplication app)
{
    app.MapGet("/shopping-cart", [Authorize] async () =>
        await GetShoppingCartClient() ?? new CustomerShoppingCart())
        .WithName("GetShoppingCart")
        .WithTags("ShoppingCart");

    app.MapPost("/shopping-cart", [Authorize] async (
        CartItem item) =>
        {
            var shoppingCart = await GetShoppingCartClient();

            if (shoppingCart == null)
                ManageNewCart(item);
            else
                ManageCart(shoppingCart, item);

            if (Errors.Any()) return CustomResponse();

            await Persist();
            return CustomResponse();
        })
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("AddItem")
        .WithTags("ShoppingCart");

    app.MapPut("/shopping-cart/{productId}", [Authorize] async (
        Guid productId,
        CartItem item) =>
        {
            var shoppingCart = await GetShoppingCartClient();
            var shoppingCartItem = await GetValidItem(productId, shoppingCart, item);
            if (shoppingCartItem == null) return CustomResponse();

            shoppingCart.UpdateUnit(shoppingCartItem, item.Quantity);

            ValidateShoppingCart(shoppingCart);
            if (Errors.Any()) return CustomResponse();

            context.CartItems.Update(shoppingCartItem);
            context.CustomerShoppingCart.Update(shoppingCart);

            await Persist();
            return CustomResponse();
        })
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("UpdateItem")
        .WithTags("ShoppingCart");

    app.MapDelete("/shopping-cart/{productId}", [Authorize] async (
        Guid productId) =>
        {
            var cart = await GetShoppingCartClient();

            var item = await GetValidItem(productId, cart);
            if (item == null) return CustomResponse();

            ValidateShoppingCart(cart);
            if (Errors.Any()) return CustomResponse();

            cart.RemoveItem(item);

            context.CartItems.Remove(item);
            context.CustomerShoppingCart.Update(cart);

            await Persist();
            return CustomResponse();
        })
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .WithName("RemoveItem")
        .WithTags("ShoppingCart");

    app.MapPost("/shopping-cart/apply-voucher", [Authorize] async (
        Voucher voucher) =>
        {
            var cart = await GetShoppingCartClient();

            cart.ApplyVoucher(voucher);

            context.CustomerShoppingCart.Update(cart);

            await Persist();
            return CustomResponse();
        })
        .ProducesValidationProblem()
        .Produces<CartItem>(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .WithName("ApplyVoucher")
        .WithTags("ShoppingCart");
}

#endregion

#region Action Methods

async Task<CustomerShoppingCart> GetShoppingCartClient()
{
    return await context.CustomerShoppingCart
        .Include(c => c.Items)
        .FirstOrDefaultAsync(c => c.CustomerId == user.GetUserId());
}

void ManageNewCart(CartItem item)
{
    var cart = new CustomerShoppingCart(user.GetUserId());
    cart.AddItem(item);

    ValidateShoppingCart(cart);
    context.CustomerShoppingCart.Add(cart);
}

void ManageCart(CustomerShoppingCart cart, CartItem item)
{
    var savedItem = cart.HasItem(item);

    cart.AddItem(item);
    ValidateShoppingCart(cart);

    if (savedItem)
    {
        context.CartItems.Update(cart.GetProductById(item.ProductId));
    }
    else
    {
        context.CartItems.Add(item);
    }

    context.CustomerShoppingCart.Update(cart);
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

    var cartItem = await context.CartItems
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
    var result = await context.SaveChangesAsync();
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
    Errors.Add(error);
}

IResult CustomResponse(object result = null)
{
    if (!Errors.Any())
    {
        return Results.Ok(result);
    }

    return Results.BadRequest(Results.ValidationProblem(
        new Dictionary<string, string[]>
        {
            { "Messages", Errors.ToArray() }
        }));
}

#endregion