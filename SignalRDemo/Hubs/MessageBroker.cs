using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRDemo.Domain.Hubs;
using System.Diagnostics;

namespace SignalRDemo.Hubs
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IConnectionManager _connectionManager;

        public MessageBroker(IConnectionManager connectionManager)
        {
            Debug.WriteLine("MessageBroker");

            _connectionManager = connectionManager;
        }

        public void ShowNewMessage(string message)
        {
            Debug.WriteLine("Message Broker: " + message);

            var context = _connectionManager.GetHubContext<MessageHub>();
            //var context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();

            // Use Hub Context and send message
            context.Clients.All.showMessageOnPage(message);
        }
    }
}