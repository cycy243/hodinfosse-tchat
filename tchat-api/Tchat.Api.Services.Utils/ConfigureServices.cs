using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tchat.Api.Services.Utils
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddUtilsServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddScoped<IEmailSender>(sp => 
                    new EmailSenderService(
                        configuration["MailConfiguration:Email"] ?? throw new ArgumentException("The mail sender shouldn't be null"),
                        configuration["MailConfiguration:Password"] ?? throw new ArgumentException("The password of the mail sender shouldn't be null"),
                        int.Parse(configuration["MailConfiguration:Port"] ?? throw new ArgumentException("The port of the mail configuration shouldn't be null")),
                        configuration["MailConfiguration:SmtpUri"] ?? throw new ArgumentException("The smtp uri shouldn't be null"), 
                        new SmtpClient())
                );
        }
    }
}
