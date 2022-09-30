using Domain.Core;
using Domain.EFEntities;

namespace Domain.Interfaces
{
    public interface IActivityService
    {
        Task<Result> GetAsync(Guid id);
        Task<Result> ListAsync();
        Task<Result> UpdateAsync(Activity activity);
        Task<Result> AddAsync(Activity activity);
        Task<Result> DeleteAsync(Guid id);
    }
}
