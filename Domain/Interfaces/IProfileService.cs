using Domain.Core;
using Domain.Enums;
using Domain.Profiles;

namespace Domain.Interfaces
{
    public interface IProfileService
    {
        Task<Result<object>> GetProfile(string userName);
        Task<Result<object>> UpdateProfile(UpdateProfile profile);
        Task<Result<object>> ListProfileActivities(string username, ProfileActivityPredicateTypeEnum predicate);
    }
}
