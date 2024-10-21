using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using Tchat.API.Policies.Requirement;

namespace Tchat.API.Policies.Handlers
{
    public class UserManagementPolicyHandler : AuthorizationHandler<PolicyRoleRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserManagementPolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PolicyRoleRequirement requirementVal)
        {
            if(context?.User?.Identity?.IsAuthenticated == false)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var queryValues = _httpContextAccessor.HttpContext.Request.Query;
            var userId = _httpContextAccessor.HttpContext.Request.RouteValues.FirstOrDefault(rv => rv.Key == "id").Value?.ToString();

            var roles = context?.User.Claims.Select(c => c.Value);
            var connectedUserId = context!.User.Claims.First(c => c.Type == "user_id").Value;
            if ((roles != null && roles.Any(requirementVal.Roles!.Contains)) || (userId != null && connectedUserId == userId && _httpContextAccessor.HttpContext.Request.Method != "POST"))
            {
                context?.Succeed(requirementVal);
                return Task.CompletedTask;
            }
            context?.Fail();
            return Task.CompletedTask;
        }
    }
}
