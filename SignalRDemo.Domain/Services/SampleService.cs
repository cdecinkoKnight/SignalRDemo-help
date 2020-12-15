using SignalRDemo.Domain.Hubs;

namespace SignalRDemo.Domain.Services
{
    public class SampleService : ISampleService
    {
        readonly IMessageBroker _messageBroker;

        public SampleService(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public string GetDummyValue()
        {
            _messageBroker.ShowNewMessage("From Service");

            return "dummy value";
        }
    }
}
