using Domain.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class ChatHub: Hub
    {
        private readonly ICommentService _commentService;

        public ChatHub(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public async Task SendComment(string body, Guid activityId)
        {
            var result = await _commentService.Create(body, activityId);

            await Clients.Group(activityId.ToString())
                .SendAsync("ReceiveComment", result.Value);
        }

        public override async Task OnConnectedAsync()
        { 
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);

            var result = await _commentService.List(Guid.Parse(activityId));

            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
