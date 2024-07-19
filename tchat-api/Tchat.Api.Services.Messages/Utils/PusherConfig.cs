using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Services.Messages.Utils
{
    public record PusherConfig(        
        string AppId,
        string AppKey,
        string AppSecret,
        string Cluster,
        bool Encrypted
    );
}
