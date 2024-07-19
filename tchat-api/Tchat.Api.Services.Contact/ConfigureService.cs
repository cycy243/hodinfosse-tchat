using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Services.Contact
{
    public static class ConfigureService
    {
        public static IServiceCollection AddContactServices(this IServiceCollection services)
        {
            return services.AddScoped<IContactService, ContactService>();
        }
    }
}
