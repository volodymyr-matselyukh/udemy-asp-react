using Domain.Core;

namespace Domain.Interfaces
{
    public interface IActivityService
    {
        Task<Result<Activity>> GetAsync(Guid id);
        Task<Result<List<Activity>>> ListAsync();
        Task<Result<object>> UpdateAsync(Activity activity);
        Task<Result<Guid>> AddAsync(Activity activity);
        Task<Result<string>> DeleteAsync(Guid id);
    }
}
