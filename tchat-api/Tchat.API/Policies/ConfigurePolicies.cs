using Microsoft.AspNetCore.Authorization;
using Tchat.API.Policies.Handlers;

namespace Tchat.API.Policies
{
    public static class ConfigurePolicies
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationHandler, UserManagementPolicyHandler>();
        }
    }
}
