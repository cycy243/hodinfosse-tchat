using Google.Apis.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Service.Auth;
using Tchat.Api.Services.Utils;

namespace Tchat.Api.Services.Auth
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddAuthServices(this IServiceCollection services, JwtConfiguration jwtConfiguration)
        {
            return services
                .AddScoped<GoogleAuthService>()
                .AddScoped<ICredentialValidator<string, GoogleJsonWebSignature.Payload>, GoogleTokenValidator>()
                .AddScoped<LocaleAuthService>()
                .AddScoped<ITokenGenerator>(sc => new TokenGenerator(jwtConfiguration));
        }
    }
}
