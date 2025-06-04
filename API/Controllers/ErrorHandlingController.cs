using API.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ErrorHandlingController : BaseApiController
    {
        [HttpGet("unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("testing: bad request");
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFound()
        {
            return NotFound();
        }

        [HttpGet("internalerror")]
        public IActionResult GetInternalError()
        {
            throw new InvalidOperationException("Test: internal error exception");
        }

        [HttpPost("validationerror")]
        public IActionResult GetValidationError(CreateProductDto productDto)
        {
            return Ok(productDto);
        }
    }
}
