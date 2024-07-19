using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Exceptions.Auth
{
    public class AuthenticationException : Exception, IException
    {
        public AuthenticationException(string message) : base(message)
        {
        }

        public string EMessage => this.Message;

        public int StatusCode => (int)StatusCodes.Status404NotFound;
    }
}
