using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Exceptions.User
{
    public class UserNotFoundException: Exception, IException
    {
        public UserNotFoundException(string message): base(message)
        {
        }

        public string EMessage => Message;

        public int StatusCode => StatusCodes.Status404NotFound;
    }
}
