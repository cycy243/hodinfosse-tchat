using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Exceptions.Utils
{
    public class InvalidArgumentException : Exception, IException
    {
        private readonly string _friendlyMsg;

        public InvalidArgumentException(string message, string friendlyMsg) : base(message)
        {
            _friendlyMsg = friendlyMsg;
        }

        public string EMessage => _friendlyMsg;

        public int StatusCode => StatusCodes.Status400BadRequest;
    }
}
