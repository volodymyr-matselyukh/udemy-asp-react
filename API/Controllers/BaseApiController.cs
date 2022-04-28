using Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResult(Result result)
        {
            if (result == null)
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                if (result.Value != null)
                {
                    return Ok(result.Value);
                }
                else if (result.IsNotFound)
                {
                    return NotFound();
                }

                return NoContent();
            }
            
            return BadRequest(result.ErrorMessage);
        }
    }
}
