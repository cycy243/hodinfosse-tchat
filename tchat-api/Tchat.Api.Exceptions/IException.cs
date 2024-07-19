using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Exceptions
{
    internal interface IException
    {
        string EMessage { get; }
        int StatusCode { get; }
    }
}
