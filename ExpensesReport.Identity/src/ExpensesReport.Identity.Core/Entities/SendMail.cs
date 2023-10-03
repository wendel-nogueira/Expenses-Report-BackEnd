using System.Text.Json;

namespace ExpensesReport.Identity.Core.Entities
{
    public class SendMail
    {
        public SendMail(string from, string to, string subject, string body, bool isBodyHtml)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
            IsBodyHtml = isBodyHtml;
        }

        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

        public string ToJson() => JsonSerializer.Serialize(this);
    }
}
