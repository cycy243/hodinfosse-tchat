using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Tchat.Api.Exceptions.Middlewares;

namespace Tchat.Api.Exceptions
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddExceptions(this IServiceCollection services)
        {
            return services.AddScoped<GlobalExceptionHandlerMiddleware>();
        }

        public static IApplicationBuilder UseExceptions(this IApplicationBuilder builder)
        {
            return builder
                .UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }
    }
}
