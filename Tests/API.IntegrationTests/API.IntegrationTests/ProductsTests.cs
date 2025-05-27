using System.Net;
using System.Net.Http.Json;
using Core.Entities;
using FluentAssertions;

namespace API.IntegrationTests
{
    public class ProductsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client = factory.CreateClient();

        private static Product GetSampleProduct(
           string name = "Mouse",
           string type = "Peripheral",
           string brand = "Logitech") => new()
           {
               Name = name,
               Description = "Gaming Mouse",
               Price = 59.99m,
               PictureUrl = "http://example.com/mouse.jpg",
               Type = type,
               Brand = brand,
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

        // !Update to test sorting logic
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
        public async Task Get_Product_Brands_Return_Unique_List()
        {
            // Arrange: ensure distinct brands
            await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ProdA", "Type1", "BrandA"));
            await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ProdB", "Type2", "BrandB"));
            await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ProdC", "Type1", "BrandA"));

            // Act
            var response = await _client.GetAsync("/api/v1/products/brands");
            response.EnsureSuccessStatusCode();
            var brands = await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>();

            // Assert
            brands.Should().Contain(["BrandA", "BrandB"]);
            brands.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Get_Product_Types_Return_Unique_List()
        {
            // Arrange: ensure distinct types
            await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ProdX", "TypeX", "BrandX"));
            await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ProdY", "TypeY", "BrandY"));
            await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ProdZ", "TypeX", "BrandZ"));

            // Act
            var response = await _client.GetAsync("/api/v1/products/types");
            response.EnsureSuccessStatusCode();
            var types = await response.Content.ReadFromJsonAsync<IReadOnlyList<string>>();

            // Assert
            types.Should().Contain(["TypeX", "TypeY"]);
            types.Should().OnlyHaveUniqueItems();
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

        [Fact]
        public async Task Delete_Product_Success()
        {
            var postResponse = await _client.PostAsJsonAsync("/api/v1/products", GetSampleProduct("ToDeleteProduct"));
            var product = await postResponse.Content.ReadFromJsonAsync<Product>();

            var deleteResponse = await _client.DeleteAsync($"/api/v1/products/{product!.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"api/v1/products/{product.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetProductById_NotFound()
        {
            var getResponse = await _client.GetAsync("/api/v1/products/99999");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateProduct_NotFound()
        {
            var nonExistingProduct = GetSampleProduct("Phantom");
            nonExistingProduct.Id = 89898;

            var getResponse = await _client.PutAsJsonAsync($"/api/v1/products/{nonExistingProduct.Id}", nonExistingProduct);
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteProduct_NotFound()
        {
            var response = await _client.DeleteAsync("/api/v1/products/77777");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProduct_InvalidModel_ReturnsBadRequest()
        {
            var badProduct = new Product
            {
                Name = "",
                Description = "",
                Price = 10.00m,
                PictureUrl = "",
                Type = "",
                Brand = "",
                QuantityInStock = 0
            };

            var response = await _client.PostAsJsonAsync("/api/v1/products", badProduct);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
