using System.Net.Http.Json;
using Core.Entities;
using FluentAssertions;

namespace API.IntegrationTests
{
    public class ProductsTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ProductsTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        private Product GetSampleProduct(string name = "Mouse") => new()
        {
            Name = name,
            Description = "Gaming Mouse",
            Price = 59.99m,
            PictureUrl = "http://example.com/mouse.jpg",
            Type = "Peripheral",
            Brand = "Logitech",
            QuantityInStock = 12
        };

        [Fact]
        public async Task Create_Then_Get_Product_Success()
        {
            var newProduct = GetSampleProduct("Keyboard");

            var postResponse = await _client.PostAsJsonAsync("/api/v1/products", newProduct);
            postResponse.EnsureSuccessStatusCode();

            var created = await postResponse.Content.ReadFromJsonAsync<Product>();
            created.Should().NotBeNull();
            created!.Id.Should().BeGreaterThan(0);

            var getResponse = await _client.GetAsync($"api/v1/products/{created!.Id}");
            getResponse.EnsureSuccessStatusCode();

            var retrieved = await getResponse.Content.ReadFromJsonAsync<Product>();
            retrieved.Should().NotBeNull();
            retrieved.Name.Should().Be("Keyboard");
            retrieved.Brand.Should().Be("Logitech");
        }
    }
}