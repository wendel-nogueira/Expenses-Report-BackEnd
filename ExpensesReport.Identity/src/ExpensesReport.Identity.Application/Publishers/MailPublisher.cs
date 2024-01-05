using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Infrastructure.Queue;

namespace ExpensesReport.Identity.Application.Publishers
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
