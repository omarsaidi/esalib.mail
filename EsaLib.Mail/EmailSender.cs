using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EsaLib.Mail
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;
        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }

        public async Task<string> SendEmailAsync(string emailTo, string[] cc, string[] bcc, string subject, string message, MailboxModel mailboxModel)
        {
            MimeMessage emailMessage = new();

            emailMessage.From.Add(new MailboxAddress(mailboxModel.MailboxName, mailboxModel.MailboxEmail));
            emailMessage.To.Add(new MailboxAddress("", emailTo));

            if (cc != null && cc.Length > 0)
            {
                foreach (string item in cc)
                {
                    emailMessage.Cc.Add(new MailboxAddress("", item));
                }
            }
            if (bcc != null && bcc.Length > 0)
            {
                foreach (string item in bcc)
                {
                    emailMessage.Bcc.Add(new MailboxAddress("", item));
                }
            }
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            // The request was aborted: Could not create SSL/TLS secure channel
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using SmtpClient client = new();
            try
            {
                client.CheckCertificateRevocation = false;
                client.Connect(mailboxModel.SmtpHostname, mailboxModel.SmtpPort, mailboxModel.EnableSSl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
                // Note: only needed if the SMTP server requires authentication
                if (!string.IsNullOrWhiteSpace(mailboxModel.SmtpUsername) && !string.IsNullOrWhiteSpace(mailboxModel.SmtpPassword))
                {
                    client.Authenticate(mailboxModel.SmtpUsername, mailboxModel.SmtpPassword);
                }

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return "AN_ERROR_HAS_OCCURRED_WHEN_TRYING_TO_SEND_AN_EMAIL";
            }
        }
    }
}
