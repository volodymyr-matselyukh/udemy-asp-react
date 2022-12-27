using Domain.Core;
using Domain.DTOs;
using Domain.EFEntities;
using Domain.Models;

namespace Domain.Interfaces
{
    public interface IActivityService
    {
        Task<Result<object>> GetAsync(Guid id);
        Task<Result<PagedList<ActivityDto>>> ListAsync(ActivityParams parameters);
        Task<Result<object>> UpdateAsync(Activity activity);
        Task<Result<object>> AddAsync(Activity activity);
        Task<Result<object>> DeleteAsync(Guid id);
    }
}
