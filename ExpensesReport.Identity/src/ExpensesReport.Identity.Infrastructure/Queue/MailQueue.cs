using Azure.Messaging.ServiceBus;
using ExpensesReport.Identity.Core.Entities;

namespace ExpensesReport.Identity.Infrastructure.Queue
{
    public class MailQueue
    {
        private readonly ServiceBusClient _client;

        public MailQueue(ServiceBusClient client)
        {
            _client = client;
        }

        public void Send(SendMail sendMail)
        {
            var message = sendMail.ToJson();
            var sender = _client.CreateSender("mail");
            var serviceBusMessage = new ServiceBusMessage(message);
            sender.SendMessageAsync(serviceBusMessage);
        }
    }
}
