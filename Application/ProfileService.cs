using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Core;
using Domain.Interfaces;
using Domain.Profiles;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application
{
    public class ProfileService : IProfileService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ProfileService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<Result> GetProfile(string userName)
        {
            var user = await _dataContext.Users
                .ProjectTo<AttendeeProfile>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Username == userName);

            return Result.Success(user);
        }
    }
}
