using System.Net.Mail;
using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;

namespace Tchat.Api.Services.Utils
{
    public class EmailSenderService : IEmailSender
    {
        private readonly string _adminMail;
        private readonly string _adminPassword;
        private readonly int _port;
        private readonly string _smtpUri;
        private readonly ISmtpClient _smtpClient;

        public EmailSenderService(string adminMail, string adminPassword, int port, string smtpUri, ISmtpClient smtpClient)
        {
            _adminMail = adminMail;
            _adminPassword = adminPassword;
            _port = port;
            _smtpUri = smtpUri;
            _smtpClient = smtpClient;
        }

        public void SendEmail(MailArg args)
        {
            MimeMessage email = CreateMailFrom(args);
            try
            {
                _smtpClient.Connect(_smtpUri, _port, SecureSocketOptions.StartTls);
                _smtpClient.Authenticate(_adminMail, _adminPassword);
                _smtpClient.Send(email);
                _smtpClient.Disconnect(true);
            }
            catch (AuthenticationException)
            {
                throw new Tchat.Api.Exceptions.Auth.AuthenticationException("Wrong credentials");
            }
            catch(InvalidOperationException)
            {
                throw new Tchat.Api.Exceptions.Auth.ConnectionException("The connection to the server failed.");
            }
        }

        private MimeMessage CreateMailFrom(MailArg args)
        {
            try
            {
                MimeMessage email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_adminMail));
                email.To.Add(MailboxAddress.Parse(args.MailAdress));
                email.Subject = args.Subject;
                email.Body = new TextPart(TextFormat.Html)
                {
                    Text = args.Message
                };
                return email;
            }
            catch(ParseException)
            {
                throw new FormatException("The given email adress is not valid.");
            }
        }
    }
}
