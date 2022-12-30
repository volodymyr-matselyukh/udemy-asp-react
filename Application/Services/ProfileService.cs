using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Core;
using Domain.DTOs;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public ProfileService(DataContext dataContext,
            IMapper mapper,
            IUserAccessor userAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }
        public async Task<Result<object>> GetProfile(string userName)
        {
            var user = await _dataContext.Users
                .ProjectTo<AttendeeProfile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                .SingleOrDefaultAsync(x => x.Username == userName);

            return Result.Success(user);
        }
        public async Task<Result<object>> UpdateProfile(UpdateProfile profile)
        {
            var user = await _dataContext.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null)
            {
                return Result.Error("The user has not been found");
            }

            user.DisplayName = profile.DisplayName;
            user.Bio = profile.Bio;

            var isSuccess = await _dataContext.SaveChangesAsync() > 0;

            if (isSuccess)
            {
                return Result.SuccessNoContent();
            }

            return Result.Error("Nothing to update");
        }

        public async Task<Result<object>> ListProfileActivities(string username, ProfileActivityPredicateTypeEnum predicate)
        {
            var activityAttendeesQuery = _dataContext.ActivityAttendees
                .Where(activityAttendee => activityAttendee.AppUser.UserName == username);

            switch (predicate)
            {
                case ProfileActivityPredicateTypeEnum.past:
                    activityAttendeesQuery = activityAttendeesQuery.Where(activityAttendee => activityAttendee.Activity.Date <= DateTime.UtcNow);
                    break;
                case ProfileActivityPredicateTypeEnum.hosting:
                    activityAttendeesQuery = activityAttendeesQuery.Where(activityAttendee => activityAttendee.IsHost);
                    break;
                case ProfileActivityPredicateTypeEnum.future:
                    activityAttendeesQuery = activityAttendeesQuery.Where(activityAttendee => activityAttendee.Activity.Date > DateTime.UtcNow);
                    break;
            }

            var profileActivities = await activityAttendeesQuery.ProjectTo<ProfileActivityDto>(_mapper.ConfigurationProvider)
                                            .ToListAsync();

            return Result.Success(profileActivities);
        }
    }
}
