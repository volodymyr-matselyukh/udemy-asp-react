using Domain.Core;

namespace Domain.Interfaces
{
    public interface IAttendenceService
    {
        Task<Result<object>> UpdateAttendeeAsyns(Guid id);
    }
}
