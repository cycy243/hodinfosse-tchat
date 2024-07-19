using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Data.Repository.DataBase
{
    public static class ConfigureService
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services.AddScoped<IUserRepository, UserDataBaseRepository>()
                .AddScoped<IMessageRepository, MessageDataBaseRepository>()
                .AddScoped<IContactRepository, ContactDataBaseRepository>();
        }
    }
}
