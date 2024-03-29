﻿using Domain.Enums;
using Domain.Interfaces;
using Domain.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        private readonly IProfileService _profileService;

        public ProfilesController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string userName)
        { 
            var result = await _profileService.GetProfile(userName);

            return HandleResult(result);
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetProfileActivities(string username, ProfileActivityPredicateTypeEnum predicate = ProfileActivityPredicateTypeEnum.all)
        {
            var result = await _profileService.ListProfileActivities(username, predicate);

            return HandleResult(result);
        }

        [Authorize(Policy = "IsUserSignedIn")]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UpdateProfile profile)
        {
            var result = await _profileService.UpdateProfile(profile);

            return HandleResult(result);
        }
    }
}
