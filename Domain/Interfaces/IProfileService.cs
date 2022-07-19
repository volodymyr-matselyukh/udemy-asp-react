using Domain.Core;

namespace Domain.Interfaces
{
    public interface IProfileService
    {
        Task<Result> GetProfile(string userName);
    }
}
