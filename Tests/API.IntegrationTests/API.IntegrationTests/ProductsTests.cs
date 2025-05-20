using System.Net;
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

        [Fact]
        public async Task Get_Products_Return_List()
        {
            var product1 = await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("Monitor"));
            var product2 = await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("Webcam"));

            var response = await _client.GetAsync("/api/v1/products");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();
            products.Should().Contain(p => p.Name == "Monitor");
            products.Should().Contain(p => p.Name == "Webcam");
        }

        [Fact]
        public async Task Update_Product_Success()
        {
            var product = await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("SSD"));
            var created = await product.Content.ReadFromJsonAsync<Product>();

            created!.Price = 99.99m;
            created.QuantityInStock = 10;

            var updateResponse = await _client.PutAsJsonAsync($"/api/v1/products/{created.Id}", created);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/v1/products/{created!.Id}");
            var updated = await getResponse.Content.ReadFromJsonAsync<Product>();

            updated!.Price.Should().Be(99.99m);
            updated.QuantityInStock.Should().Be(10);
        }
    }
}