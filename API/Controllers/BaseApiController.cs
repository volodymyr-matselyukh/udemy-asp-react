using API.Extentions;
using Domain.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        protected ActionResult HandleResult<T>(Result<T> result)
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

        protected ActionResult HandlePagedResult<T>(Result<PagedList<T>> result)
        {
            if (result == null)
            {
                return NotFound();
            }

            if (result.IsSuccess)
            {
                return HandlePagedSuccessResult(result);
            }

            return BadRequest(result.ErrorMessage);
        }

        private ActionResult HandlePagedSuccessResult<T>(Result<PagedList<T>> result)
        {
            if (result.Value != null)
            {
                Response.AddPaginationHeader(result.Value.CurrentPage,
                    result.Value.PageSize,
                    result.Value.TotalCount,
                    result.Value.TotalPages);

                return Ok(result.Value);
            }
            else if (result.IsNotFound)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
