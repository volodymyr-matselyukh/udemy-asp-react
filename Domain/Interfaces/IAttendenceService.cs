using Domain.Core;

namespace Domain.Interfaces
{
    public interface IAttendenceService
    {
        Task<Result> UpdateAttendeeAsyns(Guid id);
    }
}
