using Application.Interfaces;
using Domain;
using Domain.Core;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistance;

namespace Application
{
    public class AttendenceService : IAttendenceService
    {
        private readonly DataContext _dataContext;
        private readonly IUserAccessor _userAccessor;

        public AttendenceService(DataContext dataContext, IUserAccessor userAccessor)
        {
            _dataContext = dataContext;
            _userAccessor = userAccessor;
        }
        public async Task<Result> UpdateAttendeeAsyns(Guid id)
        {
            var activity = await _dataContext.Activities
                .Include(activity => activity.Attendees)
                .ThenInclude(attendee => attendee.AppUser)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (activity == null)
            {
                return Result.SuccessNotFound();
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(x =>
                x.UserName == _userAccessor.GetUsername());

            if (user == null)
            {
                return Result.SuccessNotFound();
            }

            var hostUserName = activity.Attendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

            var attendence = activity.Attendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

            if (attendence != null && hostUserName == user.UserName)
            {
                activity.IsCancelled = !activity.IsCancelled;
            }

            if (attendence != null && hostUserName != user.UserName) 
            {
                activity.Attendees.Remove(attendence);
            }

            if (attendence == null)
            {
                attendence = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = false
                };

                activity.Attendees.Add(attendence);
            }

            var result = await _dataContext.SaveChangesAsync() > 0;

            return result ? Result.SuccessNoContent() : Result.Error("Problem updating attendence");
        }
    }
}
