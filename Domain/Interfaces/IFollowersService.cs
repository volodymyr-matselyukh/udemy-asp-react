using Domain.Core;

namespace Domain.Interfaces
{
    public interface IFollowersService
    {
        Task<Result<object>> FollowToggle(string targetUserName);
        Task<Result<object>> List(string predicate, string username);
    }
}
