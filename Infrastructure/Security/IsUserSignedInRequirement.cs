using Domain.Interfaces;
using Domain.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infrastructure.Security
{
    public class IsUserSignedInRequirement : IAuthorizationRequirement
    {
    }

    public class IsUserSignedInRequirementHandler : AuthorizationHandler<IsUserSignedInRequirement>
    {
        private readonly IHttpContextAccessor _hpptContextAccessor;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<IsUserSignedInRequirementHandler> _logger;

        public IsUserSignedInRequirementHandler(IHttpContextAccessor hpptContextAccessor,
            IUserAccessor userAccessor,
            ILogger<IsUserSignedInRequirementHandler> logger)
        {
            _hpptContextAccessor = hpptContextAccessor;
            _userAccessor = userAccessor;
            _logger = logger;
        }
        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, IsUserSignedInRequirement requirement)
        {
            var currentUserName = _userAccessor.GetUsername();

            _logger.LogInformation("CurrentUsername {currentUserName}", currentUserName);

            if (_hpptContextAccessor.HttpContext == null)
            {
                return Task.CompletedTask;
            }

            var bodyString = await ReadRequestBody(_hpptContextAccessor.HttpContext.Request);

            var profileUpdateDto = JsonSerializer.Deserialize<UpdateProfile>(bodyString,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault
                }
            );

            if (profileUpdateDto == null)
            { 
                return Task.CompletedTask; 
            }

            _logger.LogInformation("Username from Url {usernameString}", profileUpdateDto.Username);

            if (currentUserName == profileUpdateDto.Username)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

        private async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.EnableBuffering();

            var streamReader = new StreamReader(request.Body);

            string bodyContent = await streamReader.ReadToEndAsync();

            request.Body.Position = 0;

            return bodyContent;
        }
    }
}
