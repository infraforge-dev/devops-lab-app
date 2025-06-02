using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecificationParams specParams)
        {
            var spec = new ProductSpecification(specParams);
            return await CreatePagedResult(repository, spec, specParams.PageIndex, specParams.PageSize);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return product;
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            var spec = new TypeListSpecification();
            return Ok(await repository.ListAsync<string>(spec));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            var spec = new BrandListSpecification();
            return Ok(await repository.ListAsync<string>(spec));
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repository.Add(product);

            if (await repository.SaveAllAsync())
            {
                return CreatedAtAction("GetProductById", new { id = product.Id }, product);
            }

            return BadRequest("Error creating product.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !repository.Exists(id))
            {
                return BadRequest("Cannot update this product.");
            }

            repository.Update(product);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Error updating product.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            repository.Remove(product);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Error deleting product.");
        }
    }
}
