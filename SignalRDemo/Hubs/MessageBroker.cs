using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRDemo.Domain.Hubs;
using System.Diagnostics;

namespace SignalRDemo.Hubs
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IConnectionManager _connectionManager;

        public MessageBroker(HubConfiguration hubConfiguration)
        {
            Debug.WriteLine("MessageBroker");
            _connectionManager = hubConfiguration.Resolver.Resolve<IConnectionManager>();
        }

        public void ShowNewMessage(string message)
        {
            Debug.WriteLine("Message Broker: " + message);

            var context = _connectionManager.GetHubContext<MessageHub>();
            context.Clients.All.showServiceMessageOnPage(message);
        }
    }
}