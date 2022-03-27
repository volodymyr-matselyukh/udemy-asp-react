namespace Domain.Interfaces
{
    public interface IActivityService
    {
        Task<Activity> GetAsync(Guid id);
        Task<List<Activity>> ListAsync();
        Task UpdateAsync(Activity activity);
        Task<Guid> AddAsync(Activity activity);
        Task DeleteAsync(Guid id);
    }
}
