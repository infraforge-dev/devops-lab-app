using System.Text.Json;
using Core.Entities;

namespace Infrastructure.Data
{
    public class ProductContextSeed
    {
        public static async Task SeedAsync(ProductsDbContext context)
        {
            if (!context.Products.Any())
            {
                // !Check file path if error occurs
                var productsData = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products == null)
                {
                    return;
                }

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
