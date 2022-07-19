using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PhotosController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] IFormFile file)
        { 
            var result = await _photoService.AddAsync(file);

            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        { 
            var result = await _photoService.DeleteAsync(id);

            return HandleResult(result);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            var result = await _photoService.SetMain(id);

            return HandleResult(result);
        }
    }
}
