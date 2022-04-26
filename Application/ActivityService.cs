using Domain;
using Domain.Core;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application;
public class ActivityService : IActivityService
{
    private readonly DataContext _dataContext;

    public ActivityService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    public async Task<Result<Guid>> AddAsync(Activity activity)
    {
        _dataContext.Activities.Add(activity);
        var isRecordChanged = await _dataContext.SaveChangesAsync() > 0;

        if (!isRecordChanged)
        {
            return Result<Guid>.Failure("Failed to create activity");
        }

        return Result<Guid>.Success(activity.Id);
    }

    public async Task<Result<string>> DeleteAsync(Guid id)
    {
        var activity = await _dataContext.Activities.FindAsync(id);

        if (activity != null)
        {
            _dataContext.Activities.Remove(activity);

            var isRowsAffected = await _dataContext.SaveChangesAsync() > 0;

            if (!isRowsAffected)
            {
                return Result<string>.Failure("Failed to delete activity");
            }
        }
        else
        { 
            return null;
        }

        return Result<string>.EmptySuccess();
    }

    public async Task<Result<Activity>> GetAsync(Guid id)
    {
        var activity = await _dataContext.Activities.FindAsync(id);

        return Result<Activity>.Success(activity);
    }

    public async Task<Result<List<Activity>>> ListAsync()
    {
        var activities = await _dataContext.Activities.ToListAsync();

        return Result<List<Activity>>.Success(activities);
    }

    public async Task<Result<dynamic>> UpdateAsync(Activity activity)
    {
        _dataContext.Activities.Update(activity);
        await _dataContext.SaveChangesAsync();

        return Result<dynamic>.Success(null);
    }
}
