using Domain.Core;

namespace Domain.Interfaces
{
    public interface ICommentService
    {
        Task<Result> Create(string body, Guid activityId);
        Task<Result> List(Guid activityId);
    }
}
