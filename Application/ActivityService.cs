using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Core;
using Domain.DTOs;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application;
public class ActivityService : IActivityService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IUserAccessor _userAccessor;

    public ActivityService(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }
    public async Task<Result> AddAsync(Activity activity)
    {
        var user = await _dataContext.Users
            .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

        var attendee = new ActivityAttendee
        {
            AppUser = user,
            Activity = activity,
            IsHost = true
        };

        activity.Attendees.Add(attendee);

        _dataContext.Activities.Add(activity);
        var isRecordChanged = await _dataContext.SaveChangesAsync() > 0;

        if (!isRecordChanged)
        {
            return Result.Error("Failed to create activity");
        }

        return Result.Success(activity.Id);
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var activity = await _dataContext.Activities.FindAsync(id);

        _dataContext.Activities.Remove(activity);

        var isRowsAffected = await _dataContext.SaveChangesAsync() > 0;

        if (!isRowsAffected)
        {
            return Result.Error("Failed to delete activity");
        }

        return Result.SuccessNoContent();
    }

    public async Task<Result> GetAsync(Guid id)
    {
        var activity = await _dataContext.Activities
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);

        return Result.Success(activity);
    }

    public async Task<Result> ListAsync()
    {
        var activities = await _dataContext.Activities
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return Result.Success(activities);
    }

    public async Task<Result> UpdateAsync(Activity activity)
    {
        var activityFromDb = await _dataContext.Activities.FindAsync(activity.Id);

        _mapper.Map(activity, activityFromDb);
        
        var isRowAffected = await _dataContext.SaveChangesAsync() > 0;

        if (!isRowAffected)
        {
            return Result.Error("Failed to update activity");
        }

        return Result.SuccessNoContent();
    }
}
