using SignalRDemo.Domain.Hubs;
using System;

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
            _messageBroker.ShowNewMessage("From Service: " + DateTime.Now);

            return "dummy value";
        }
    }
}
