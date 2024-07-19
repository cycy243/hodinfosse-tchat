using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Services.Utils.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetUserIp(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}
