using Core.Interfaces;
using Core.RedisModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController(ICartService cartService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var cart = await cartService.GetCartAsync(id);

            return Ok(cart ?? new ShoppingCart { CartId = id });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart(ShoppingCart cart)
        {
            var updatedCart = await cartService.SetCartAsync(cart);

            if (updatedCart == null)
            {
                return BadRequest("Problem with cart.");
            }

            return updatedCart;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var result = await cartService.DeleteCartAsync(id);

            if (!result)
            {
                return BadRequest("Problem deleting cart.");
            }

            return Ok();
        }
    }
}
