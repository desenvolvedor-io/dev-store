using DevStore.Catalog.API;
using DevStore.Catalog.API.Data;
using DevStore.Catalog.API.Models;

namespace DevStore.Tests.CatalogApi;

public abstract class CatalogIntegrationTests()
    : IntegrationTest<Program>("http://localhost/catalog/")
{
    protected async Task AddProducts(params Product[] products)
    {
        await ExecuteInScope<CatalogContext>(async ctx =>
        {
            foreach (var product in products) ctx.Products.Add(product);

            await ctx.SaveChangesAsync();
        });
    }
}