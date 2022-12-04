using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FollowController : BaseApiController
    {
        private readonly IFollowersService _followersService;

        public FollowController(IFollowersService followersService)
        {
            _followersService = followersService;
        }

        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await _followersService.FollowToggle(username));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowes(string username, string predicate)
        {
            return HandleResult(await _followersService.List(predicate, username));
        }
    }
}
