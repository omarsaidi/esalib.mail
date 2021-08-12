using System.Threading.Tasks;

namespace EsaLib.Mail
{
    public interface IEmailSender
    {
        Task<string> SendEmailAsync(string emailTo, string[] cc, string[] bcc, string subject, string message, MailboxModel mailboxModel);
    }
}
