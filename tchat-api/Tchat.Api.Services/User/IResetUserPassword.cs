using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Services.User
{
    public interface IResetUserPassword
    {
        Task<bool> SendResetPasswordLinkToUser(string email);

        Task<bool> ResetPassword(string email, string code, string newPassword);
    }
}
