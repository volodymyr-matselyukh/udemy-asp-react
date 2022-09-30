using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Core;
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
        public async Task<Result> GetProfile(string userName)
        {
            var user = await _dataContext.Users
                .ProjectTo<AttendeeProfile>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Username == userName);

            return Result.Success(user);
        }
        public async Task<Result> UpdateProfile(UpdateProfile profile)
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
    }
}
