using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController(IProductRepository repository) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Unfortunately, you lose type checking by wrapping in OKObjectResult
            return Ok(await repository.GetProductsAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return product;
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await repository.GetTypesAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await repository.GetBrandsAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repository.AddProduct(product);

            if (await repository.SaveChangesAsync())
            {
                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }

            return BadRequest("Error creating product.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !repository.ProductExists(id))
            {
                return BadRequest("Cannot update this product.");
            }

            repository.UpdateProduct(product);

            if (await repository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Error updating product.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            repository.DeleteProduct(product);

            if (await repository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Error deleting product.");
        }
    }
}
