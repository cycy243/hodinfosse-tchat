using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.API.Args
{
    public record ContactQuestionSearchArgs(bool IsDeleted = false, int? Count = null);
}
