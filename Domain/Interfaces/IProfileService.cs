using Domain.Core;
using Domain.Profiles;

namespace Domain.Interfaces
{
    public interface IProfileService
    {
        Task<Result> GetProfile(string userName);
        Task<Result> UpdateProfile(UpdateProfile profile);
    }
}
