﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Core;
using Domain.DTOs;
using Domain.EFEntities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application.Services;
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
    public async Task<Result<object>> AddAsync(Activity activity)
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

    public async Task<Result<object>> DeleteAsync(Guid id)
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

    public async Task<Result<object>> GetAsync(Guid id)
    {
        var activity = await _dataContext.Activities
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
            .FirstOrDefaultAsync(x => x.Id == id);

        return Result.Success(activity);
    }

    public async Task<Result<PagedList<ActivityDto>>> ListAsync(PagingParams parameters)
    {
        var query = _dataContext.Activities
            .OrderByDescending(activity => activity.Date)
            .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = _userAccessor.GetUsername() })
            .AsQueryable();

        var activities = await PagedList<ActivityDto>.CreateAsync(
            query,
            parameters.PageNumber,
            parameters.PageSize
        );

        return Result<PagedList<ActivityDto>>.Success(activities);
    }

    public async Task<Result<object>> UpdateAsync(Activity activity)
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
