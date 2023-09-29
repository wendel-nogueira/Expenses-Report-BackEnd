namespace ExpensesReport.Mail.Core.Entities
{
    public class MailToSend
    {
        public string? From { get; set; }
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public bool IsBodyHtml { get; set; }
    }
}
