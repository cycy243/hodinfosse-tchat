using Microsoft.AspNetCore.Authorization;

namespace Tchat.API.Policies.Requirement
{
    public class PolicyRoleRequirement: IAuthorizationRequirement
    {
        //holds the array of roles
        public string[]? Roles { get; }
        public PolicyRoleRequirement(params string[] roles)
        {
            this.Roles = roles;
        }
    }
}
