using Microsoft.Extensions.DependencyInjection;
using PusherServer;
using Tchat.Api.Services.Messages.Utils;

namespace Tchat.Api.Services.Messages
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddPusherMessageServices(this IServiceCollection services, PusherConfig pusherConfig)
        {
            return services
                .AddScoped<IPusher>(sp => {
                    var options = new PusherOptions
                    {
                        Cluster = "eu",
                        Encrypted = true
                    };

                    return new Pusher(
                      pusherConfig.AppId,
                      pusherConfig.AppKey,
                      pusherConfig.AppSecret,
                      options
                    );
                })
                .AddScoped(sp => pusherConfig)
                .AddScoped<IMessageService, PusherMessageService>();
        }
    }
}
