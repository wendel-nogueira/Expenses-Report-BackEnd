using Azure.Messaging.ServiceBus;
using ExpensesReport.Expenses.Core.Entities;

namespace ExpensesReport.Expenses.Infrastructure.Queue
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
            try
            {
                var message = sendMail.ToJson();
                var sender = _client.CreateSender("mail");
                var serviceBusMessage = new ServiceBusMessage(message);
                sender.SendMessageAsync(serviceBusMessage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
