using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tchat.Api.Services
{
    /// <summary>
    /// Interface that expose common methods to send mail.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send an email with the given args.
        /// </summary>
        /// <param name="args">Mail informations</param>
        void SendEmail(MailArg args);
    }

    /// <summary>
    /// Represents the content of an email.
    /// </summary>
    /// <param name="MailAdress">Adresse of the person that will receive the mail</param>
    /// <param name="Subject">Subject of the mail</param>
    /// <param name="Message">Message that the mail will contain</param>
    public record MailArg(string MailAdress, string Subject, string Message, string Firstname = "", string Lastname = "");
}
