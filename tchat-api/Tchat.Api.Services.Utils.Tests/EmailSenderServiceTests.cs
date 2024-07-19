using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Moq;

namespace Tchat.Api.Services.Utils.Tests
{
    [TestClass]
    public class EmailSenderServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void WhenMailInformationNotRightThenThrowsFormatException()
        {
            // Arrange
            Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();
            var service = new EmailSenderService("", "", 0, "", smtpClientMock.Object);

            // Act
            service.SendEmail(new MailArg("", "", ""));
        }

        [TestMethod]
        [ExpectedException(typeof(Exceptions.Auth.AuthenticationException))]
        public void WhenAuthenticationFailThenThrowAuthenticationException()
        {
            // Arrange
            Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();
            smtpClientMock.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Throws(new AuthenticationException(""));
            var service = new EmailSenderService("mail.trest@outlook.com", "", 0, "", smtpClientMock.Object);

            // Act
            service.SendEmail(new MailArg("mail.trest@outlook.com", "vgfdgdfgdf", "edrgetgret"));
        }

        [TestMethod]
        [ExpectedException(typeof(Exceptions.Auth.ConnectionException))]
        public void WhenConnectionFailedThenThrowsConnectionException()
        {
            // Arrange
            Mock<ISmtpClient> smtpClientMock = new Mock<ISmtpClient>();
            smtpClientMock.Setup(x => x.Connect(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SecureSocketOptions>(), It.IsAny<CancellationToken>())).Throws(new InvalidOperationException(""));
            var service = new EmailSenderService("mail.trest@outlook.com", "", 0, "", smtpClientMock.Object);

            // Act
            service.SendEmail(new MailArg("mail.trest@outlook.com", "vgfdgdfgdf", "edrgetgret"));
        }
    }
}