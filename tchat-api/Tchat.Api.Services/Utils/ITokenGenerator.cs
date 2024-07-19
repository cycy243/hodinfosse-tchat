using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tchat.Api.Domain;

namespace Tchat.Api.Services.Utils
{
    public interface ITokenGenerator
    {
        string GenerateToken(Tchat.Api.Domain.User user, IEnumerable<string> roles);
    }
}
