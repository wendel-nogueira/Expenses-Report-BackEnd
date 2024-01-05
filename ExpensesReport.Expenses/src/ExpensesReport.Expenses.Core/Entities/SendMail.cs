using System.Text.Json;

namespace ExpensesReport.Expenses.Core.Entities
{
    public class SendMail
    {
        public SendMail(string? to, string? subject, string? title, string? userName, string? body, bool? showAction, string? actionText, string? actionUrl)
        {
            To = to;
            Subject = subject;
            Title = title;
            UserName = userName;
            Body = body;
            ShowAction = showAction;
            ActionText = actionText;
            ActionUrl = actionUrl;
        }

        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public string? Body { get; set; }
        public bool? ShowAction { get; set; }
        public string? ActionText { get; set; }
        public string? ActionUrl { get; set; }

        public string ToJson() => JsonSerializer.Serialize(this);
    }
}
