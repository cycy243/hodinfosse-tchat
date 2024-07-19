using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Services.Args
{
    public class UserSearchArgs
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
    }
}
