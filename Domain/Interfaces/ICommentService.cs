using Domain.Core;
using Domain.DTOs;

namespace Domain.Interfaces
{
    public interface ICommentService
    {
        Task<Result<CommentDto>> Create(string body, Guid activityId);
        Task<Result<object>> List(Guid activityId);
    }
}
