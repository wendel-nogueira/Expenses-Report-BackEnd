using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Infrastructure.Queue;
using System.Text;

namespace ExpensesReport.Identity.Application.Publishers
{
    public class MailPublisher(MailQueue mailQueue)
    {
        private readonly MailQueue _mailQueue = mailQueue;

        public void SendAddPasswordMail(string id, string email, string token)
        {
            var messageBody = $"Identity Id: {id}\n Email: {email}\n Token: {token}";
            var messageBodyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(messageBody));
            var sendMail = new SendMail("your_email@gmail.com", email, "Create password", messageBodyBase64, true);

            _mailQueue.Send(sendMail);
        }
    }
}
