
using System.Web;
using Tchat.Api.Data.Repository;
using Tchat.Api.Exceptions.User;

namespace Tchat.Api.Services.User
{
    public class ResetUserPasswordService : IResetUserPassword
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        public ResetUserPasswordService(IUserRepository userRepository, IEmailSender emailSender) 
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
        }

        public async Task<bool> SendResetPasswordLinkToUser(string email)
        {
            var user = await _userRepository.GetUserByLogin(email);
            if (user == null)
            {
                throw new UserNotFoundException($"No user found for the email: {email}");
            }
            _emailSender.SendEmail(new MailArg(user.Email, "Reset Password", GenerateResetPasswordLink(email, await _userRepository.GenerateResetPasswordTokenForUser(user))));
            return true;
        }

        public async Task<bool> ResetPassword(string email, string code, string newPassword)
        {
            var user = await _userRepository.GetUserByLogin(email);
            if (user == null)
            {
                throw new UserNotFoundException($"No user found for the email: {email}");
            }
            if (await _userRepository.ResetPassword(user, code, newPassword))
            {
                _emailSender.SendEmail(new MailArg(user.Email, "Reset Password", "Your password has been successfully changed"));
                return true;
            }
            return false;
        }

        private string GenerateResetPasswordLink(string email, string code)
        {
            return $"Click here to reset your password: <a href=\"http://localhost:5173/reset-password?code={HttpUtility.UrlEncode(code)}&email={email}\">Click here</a>";
        }
    }
}
