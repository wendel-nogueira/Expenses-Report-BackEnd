using ExpensesReport.Mail.Core.Entities;

namespace ExpensesReport.Mail.Application.Services
{
    public interface IMailServices
    {
        void SendMail(MailToSend mailToSend);
    }
}
