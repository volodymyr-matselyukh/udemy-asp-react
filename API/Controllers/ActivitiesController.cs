using Domain.Core;
using Domain.EFEntities;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly IActivityService _activityService;
        private readonly IAttendenceService _attendenceService;

        public ActivitiesController(DataContext dbContext, IActivityService activityService,
            IAttendenceService attendenceService)
        {
            _activityService = activityService;
            _attendenceService = attendenceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery]ActivityParams parameters)
        {
            var result = await _activityService.ListAsync(parameters);

            return HandlePagedResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id)
        {
            var result = await _activityService.GetAsync(id);

            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddActivity(Activity activity)
        {
            var activityId = await _activityService.AddAsync(activity);
            return CreatedAtAction("GetActivity", new { id = activityId }, null);
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            var result = await _activityService.UpdateAsync(activity);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteActivity(Guid id)
        {
            var result = await _activityService.DeleteAsync(id);
            return HandleResult(result);
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        { 
            return HandleResult(await _attendenceService.UpdateAttendeeAsyns(id));
        }
    }
}
