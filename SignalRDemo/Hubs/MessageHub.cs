using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SignalRDemo.Domain.Hubs;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SignalRDemo.Hubs
{
    [HubName("messages")]
    public class MessageHub : Hub, IMessageBroker
    {
        public override Task OnConnected()
        {
            Debug.WriteLine("Connected: " + Context.ConnectionId);
            return base.OnConnected();
        }

        public void ShowNewMessage(string message)
        {
            Debug.WriteLine("Show New Message: " + message);

            Clients.All.showControllerMessageOnPage("Show New Message: " + message);
        }
    }
}