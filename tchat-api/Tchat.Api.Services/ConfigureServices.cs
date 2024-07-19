using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Models;
using Tchat.Api.Services.Validators;

namespace Tchat.Api.Services
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddScoped<IValidator<UserDto>, UserValidator>()
                .AddScoped<IValidator<ContactQuestionDto>, ContactQuestionValidator>();
        }
    }
}
