using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Models;
using Tchat.Api.Services.Args;

namespace Tchat.Api.Services.User
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUserServices(this IServiceCollection services)
        {
            return services.AddScoped<IResetUserPassword, ResetUserPasswordService>()
                .AddScoped<IDomainService<UserDto, UserSearchArgs>, UserDomainService>();
        }
    }
}
