using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Persistance;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private DataContext _dbContext;
        private readonly IActivityService _activityService;

        public ActivitiesController(DataContext dbContext, IActivityService activityService)
        {
            _dbContext = dbContext;
            _activityService = activityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            var result = await _activityService.ListAsync();

            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            var result = await _activityService.GetAsync(id);

            return HandleResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddActivity(Activity activity)
        {
            var activityId = await _activityService.AddAsync(activity);
            return CreatedAtAction("GetActivity", new { id= activityId }, null);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            await _activityService.UpdateAsync(activity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(Guid id)
        {
            var result = await _activityService.DeleteAsync(id);
            return HandleResult(result);
        }
    }
}
