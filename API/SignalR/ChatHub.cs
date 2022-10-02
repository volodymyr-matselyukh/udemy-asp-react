using API.DTOs;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public async Task SendComment(SendCommentDto comment)
        {
            var result = await _commentService.Create(comment.Body, comment.ActivityId);

            await Clients.Group(comment.ActivityId.ToString())
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
