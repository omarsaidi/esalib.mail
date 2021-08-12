namespace EsaLib.Mail
{
    public class MailboxModel
    {
        public string MailboxName { get; set; }
        public string MailboxEmail { get; set; }
        public string SmtpHostname { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public bool EnableSSl { get; set; }
    }
}
