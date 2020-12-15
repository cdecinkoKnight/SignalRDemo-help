namespace SignalRDemo.Domain.Hubs
{
    public interface IMessageBroker
    {
        void ShowNewMessage(string message);
    }
}
