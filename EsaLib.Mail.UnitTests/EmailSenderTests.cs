using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EsaLib.Mail.UnitTests
{
    public class EmailSenderTests
    {
        private readonly Mock<ILogger<EmailSender>> _logger;

        public EmailSenderTests()
        {
            _logger = new Mock<ILogger<EmailSender>>();
        }
        [Fact]
        public async Task Send_email_return_success()
        {
            // Arrange
            MailboxModel mailboxModel = new()
            {
                MailboxEmail = "",
                MailboxName = "",
                SmtpHostname = "smtp.gmail.com",
                SmtpPort = 587,
                SmtpUsername = "",
                SmtpPassword = "",
                EnableSSl = true,
            };

            // Act
            EmailSender emailSender = new(_logger.Object);
            string result = await emailSender.SendEmailAsync("saidiomar.saidi@gmail.com", null, null, "Test send email", "Test send email body", mailboxModel);
            Assert.Empty(result);
        }
    }
}
