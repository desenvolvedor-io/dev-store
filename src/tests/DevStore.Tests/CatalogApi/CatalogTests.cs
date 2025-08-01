using System.Net.Http.Json;
using DevStore.Catalog.API.Models;

namespace DevStore.Tests.CatalogApi;

public class CatalogTests : CatalogIntegrationTests
{
    public class GetTests : CatalogTests
    {
        [Fact]
        public async Task NoParameters_Success()
        {
            // Act
            var response = await HttpClient.GetAsync("products");

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PagedResult<Product>>();
            Assert.NotNull(result);
            Assert.NotEmpty(result.List);
            Assert.Equal(8, result.List.Count());
            Assert.Single(result.List, item => item.Name == "Mug Batman" && item.Price == 50.00m);
        }
    }
}