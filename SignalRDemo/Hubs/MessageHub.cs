using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Diagnostics;

namespace SignalRDemo.Hubs
{
    [HubName("messages")]
    public class MessageHub : Hub
    {
        public void ShowNewMessage(string message)
        {
            Debug.WriteLine("Show New Message: " + message);

            Clients.All.showMessageOnPage("Show New Message: " + message);
        }
    }
}