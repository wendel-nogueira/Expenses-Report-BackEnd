using ExpensesReport.Expenses.Core.Entities;
using ExpensesReport.Expenses.Infrastructure.Queue;

namespace ExpensesReport.Expenses.Application.Publishers
{
    public class MailPublisher(MailQueue mailQueue)
    {
        private readonly MailQueue _mailQueue = mailQueue;

        public void SendMail(string? to, string? subject, string? title, string? userName, string? body, bool? showAction, string? actionText, string? actionUrl)
        {
            var sendMail = new SendMail(to, subject, title, userName, body, showAction, actionText, actionUrl);

            _mailQueue.Send(sendMail);
        }
    }
}
