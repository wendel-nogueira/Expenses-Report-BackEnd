using ExpensesReport.Mail.Application.Configurations;
using ExpensesReport.Mail.Application.Templates;
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
            _logger.LogInformation($"Sending mail");
            _logger.LogInformation($"From: {_smtpConfiguration.From}");
            _logger.LogInformation($"To: {mailToSend.To}");

            try
            {
                MailTemplate mailTemplate = new(mailToSend.Subject!, mailToSend.Title!, mailToSend.UserName!, mailToSend.Body!, mailToSend.ShowAction, mailToSend.ActionText, mailToSend.ActionUrl);
                MailMessage mailMessage = new()
                {
                    From = new MailAddress(_smtpConfiguration.From!)
                };
                mailMessage.To.Add(mailToSend.To!);
                mailMessage.Subject = mailTemplate.Subject;
                mailMessage.Body = mailTemplate.GetTemplate();
                mailMessage.IsBodyHtml = true;

                SmtpClient smtpClient = new(_smtpConfiguration.Host, _smtpConfiguration.Port)
                {
                    Credentials = new System.Net.NetworkCredential(_smtpConfiguration.Username, _smtpConfiguration.Password),
                    EnableSsl = _smtpConfiguration.EnableSsl
                };

                smtpClient.Send(mailMessage);

                _logger.LogInformation($"Mail sent");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending mail");
            }
        }
    }
}
