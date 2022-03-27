using Domain;
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
    public async Task<Guid> AddAsync(Activity activity)
    {
        _dataContext.Activities.Add(activity);
        await _dataContext.SaveChangesAsync();

        return activity.Id;
    }

    public async Task DeleteAsync(Guid id)
    {
        var activity = await _dataContext.Activities.FindAsync(id);

        if (activity != null)
        {
            _dataContext.Activities.Remove(activity);

            await _dataContext.SaveChangesAsync();
        }
    }

    public async Task<Activity> GetAsync(Guid id)
    {
        return await _dataContext.Activities.FindAsync(id);
    }

    public async Task<List<Activity>> ListAsync()
    {
        return await _dataContext.Activities.ToListAsync();
    }

    public async Task UpdateAsync(Activity activity)
    {
        _dataContext.Activities.Update(activity);
        await _dataContext.SaveChangesAsync();
    }
}
