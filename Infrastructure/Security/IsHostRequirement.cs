using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Persistance;
using System.Security.Claims;

namespace Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _hpptContextAccessor;

        public IsHostRequirementHandler(DataContext dataContext, IHttpContextAccessor hpptContextAccessor)
        {
            _dataContext = dataContext;
            _hpptContextAccessor = hpptContextAccessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Task.CompletedTask;
            }

            var activityIdString = _hpptContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value?.ToString();

            Guid activityId = Guid.Empty;

            if (!string.IsNullOrEmpty(activityIdString)) {
                activityId = Guid.Parse(activityIdString);
            }

            var attendee = _dataContext.ActivityAttendees
                .AsNoTracking()
                .FirstOrDefaultAsync(aa => aa.AppUserId == userId && aa.ActivityId == activityId)
                .Result;

            if (attendee == null)
            {
                return Task.CompletedTask;
            }

            if (attendee.IsHost)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
