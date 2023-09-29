using ExpensesReport.Mail.Application.Services;
using ExpensesReport.Mail.Core.Entities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExpensesReport.Mail.Infrastructure
{
    public class SendMail
    {
        private readonly ILogger<SendMail> _logger;
        private readonly IMailServices _mailServices;

        public SendMail(ILogger<SendMail> logger, IMailServices mailServices)
        {
            _logger = logger;
            _mailServices = mailServices;
        }

        [Function(nameof(SendMail))]
        public void Run([ServiceBusTrigger("mail", Connection = "ServiceBusConnection")] string message)
        {
            _logger.LogInformation(string.Format("New message received"));

            var mailMessage = JsonConvert.DeserializeObject<MailToSend>(message);

            if (mailMessage == null)
            {
                _logger.LogError("Error deserializing message");
                return;
            }

            if (mailMessage.IsBodyHtml)
            {
                mailMessage.Body = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(mailMessage.Body!));
            }

            if (mailMessage == null)
            {
                _logger.LogError("Error deserializing message");
                return;
            }

            try
            {
                _mailServices.SendMail(mailMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending mail");
            }
        }
    }
}