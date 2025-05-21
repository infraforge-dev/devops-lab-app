using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        // Get list of products
        Task<IReadOnlyList<Product>> GetProductsAsync();
        // Get single product
        Task<Product?> GetProductByIdAsync(int id);
        // Add a product
        void AddProduct(Product product);
        // Update a product
        void UpdateProduct(Product product);
        // Delete a product
        void DeleteProduct(Product product);
        // Save changes
        bool ProductExists(int id);
        Task<bool> SaveChangesAsync();
    }
}