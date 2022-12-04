using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Core;
using Domain.EFEntities;
using Domain.Interfaces;
using Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Services
{
    public class FollowersService: IFollowersService
    {
        private readonly DataContext _dataContext;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public FollowersService(DataContext dataContext, IUserAccessor userAccessor, IMapper mapper)
        {
            _dataContext = dataContext;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result> FollowToggle(string targetUserName)
        {
            var observer = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            var target = await _dataContext.Users.FirstOrDefaultAsync(x => x.UserName == targetUserName);

            if (target == null)
            { 
                return Result.SuccessNotFound();
            }

            var following = await _dataContext.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                _dataContext.UserFollowings.Add(following);
            }
            else
            { 
                _dataContext.UserFollowings.Remove(following);
            }

            var success = await _dataContext.SaveChangesAsync() > 0;

            if (success)
            {
                return Result.SuccessNoContent();
            }

            return Result.Error("Failed to update following");
        }

        public async Task<Result> List(string predicate, string username)
        {
            var profiles = new List<AttendeeProfile>();

            switch (predicate)
            {
                case "followers":
                    profiles = await _dataContext.UserFollowings.Where(x => x.Target.UserName == username)
                        .Select(u => u.Observer)
                        .ProjectTo<AttendeeProfile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername()})
                        .ToListAsync();
                    break;
                case "following":
                    profiles = await _dataContext.UserFollowings.Where(x => x.Observer.UserName == username)
                        .Select(u => u.Target)
                        .ProjectTo<AttendeeProfile>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
                        .ToListAsync();
                    break;
            }

            return Result.Success(profiles);
        }
    }
}
