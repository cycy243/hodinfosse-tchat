using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Exceptions.Utils
{
    public class RessourceNotFound : Exception, IException
    {
        public string EMessage => Message;

        public int StatusCode => StatusCodes.Status404NotFound;

        public RessourceNotFound(string message): base(message)
        {
        }
    }
}
