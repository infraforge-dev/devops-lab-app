using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRepository(ProductsDbContext context) : IProductRepository
    {
        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IReadOnlyList<string>> GetTypesAsync()
        {
            return await context.Products.Select(p => p.Type)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IReadOnlyList<string>> GetBrandsAsync()
        {
            return await context.Products.Select(p => p.Brand)
                .Distinct()
                .ToListAsync();
        }

        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            context.Entry(product).State = EntityState.Modified;
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public bool ProductExists(int id)
        {
            return context.Products.Any(p => p.Id == id);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }

    }
}