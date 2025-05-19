using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductsDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
