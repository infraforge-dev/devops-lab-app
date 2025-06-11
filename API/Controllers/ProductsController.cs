using API.RequestHelpers;
using Core.Entities;
using Core.ExceptionTypes;
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
            var product = await repository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Product with id {id} not found.");

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

            throw new OperationFailedException("Product could not be created.");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id || !repository.Exists(id))
            {
                throw new NotFoundException($"Product with id {id} not found.");
            }

            repository.Update(product);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            throw new OperationFailedException("Product could not be updated.");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Product with id {id} not found.");

            repository.Remove(product);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            throw new OperationFailedException("Product could not be deleted.");
        }
    }
}
