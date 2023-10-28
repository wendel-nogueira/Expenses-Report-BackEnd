using ExpensesReport.Identity.Core.Entities;
using ExpensesReport.Identity.Infrastructure.Queue;

namespace ExpensesReport.Identity.Application.Publishers
{
    public class MailPublisher(MailQueue mailQueue)
    {
        private readonly MailQueue _mailQueue = mailQueue;

        public void SendResetPasswordMail(UserIdentity identity)
        {
            var messageBody = $"Identity Id: {identity.Id}\n Email: {identity.Email}\n Token: {identity.ResetPasswordToken}";
            var sendMail = new SendMail("your_email@gmail.com", identity.Email!, "Create password", messageBody, false);

            _mailQueue.Send(sendMail);
        }
    }
}
