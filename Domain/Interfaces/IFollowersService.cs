using Domain.Core;

namespace Domain.Interfaces
{
    public interface IFollowersService
    {
        Task<Result> FollowToggle(string targetUserName);
        Task<Result> List(string predicate, string username);
    }
}
