using ExpensesReport.Mail.Application.Configurations;
using ExpensesReport.Mail.Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;

namespace ExpensesReport.Mail.Application.Services
{
    public class MailServices : IMailServices
    {
        private readonly ILogger<MailServices> _logger;
        private readonly SmtpConfiguration _smtpConfiguration;

        public MailServices(ILogger<MailServices> logger, IOptions<SmtpConfiguration> smtpConfiguration)
        {
            _smtpConfiguration = smtpConfiguration.Value;
            _logger = logger;
        }

        public void SendMail(MailToSend mailToSend)
        {
            _logger.LogInformation($"Sending mail to {mailToSend.To}");
            _logger.LogInformation($"SMTP: {_smtpConfiguration.Host}");

            try
            {
                MailMessage mailMessage = new(mailToSend.From!, mailToSend.To!, mailToSend.Subject, mailToSend.Body);

                SmtpClient smtpClient = new(_smtpConfiguration.Host, _smtpConfiguration.Port)
                {
                    Credentials = new System.Net.NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password),
                    EnableSsl = _smtpConfiguration.EnableSsl
                };

                smtpClient.Send(mailMessage);

                _logger.LogInformation($"Mail sent to {mailToSend.To}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending mail");
            }
        }
    }
}
