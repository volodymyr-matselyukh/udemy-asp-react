using AutoMapper;
using Domain;
using Domain.Core;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application;
public class ActivityService : IActivityService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ActivityService(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        this._mapper = mapper;
    }
    public async Task<Result> AddAsync(Activity activity)
    {
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

        //if (activity != null)
        //{
            _dataContext.Activities.Remove(activity);

            var isRowsAffected = await _dataContext.SaveChangesAsync() > 0;

            if (!isRowsAffected)
            {
                return Result.Error("Failed to delete activity");
            }

            return Result.SuccessNoContent();
        //}
        
        return Result.SuccessNotFound();
    }

    public async Task<Result> GetAsync(Guid id)
    {
        var activity = await _dataContext.Activities.FindAsync(id);

        return Result.Success(activity);
    }

    public async Task<Result> ListAsync()
    {
        var activities = await _dataContext.Activities.ToListAsync();

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
