using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Mappers
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddMappers(this IServiceCollection services)
        {
            return services.AddAutoMapper(typeof(ConfigureServices).Assembly);
        }
    }
}
