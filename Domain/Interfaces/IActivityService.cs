using Domain.Core;
using Domain.DTOs;
using Domain.EFEntities;

namespace Domain.Interfaces
{
    public interface IActivityService
    {
        Task<Result<object>> GetAsync(Guid id);
        Task<Result<PagedList<ActivityDto>>> ListAsync(PagingParams parameters);
        Task<Result<object>> UpdateAsync(Activity activity);
        Task<Result<object>> AddAsync(Activity activity);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
