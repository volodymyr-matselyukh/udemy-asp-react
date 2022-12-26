using Domain.Core;
using Domain.Profiles;

namespace Domain.Interfaces
{
    public interface IProfileService
    {
        Task<Result<object>> GetProfile(string userName);
        Task<Result<object>> UpdateProfile(UpdateProfile profile);
    }
}
