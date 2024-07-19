using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Exceptions.Auth
{
    public class ConnectionException: Exception, IException
    {
        public ConnectionException(string message) : base(message)
        {
        }

        public string EMessage => this.Message;

        public int StatusCode => (int)StatusCodes.Status500InternalServerError;
    }
}
