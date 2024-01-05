namespace ExpensesReport.Mail.Core.Entities
{
    public class MailToSend
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public string? Body { get; set; }
        public bool? ShowAction { get; set; }
        public string? ActionText { get; set; }
        public string? ActionUrl { get; set; }
    }
}