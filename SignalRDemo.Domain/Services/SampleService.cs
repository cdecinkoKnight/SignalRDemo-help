namespace SignalRDemo.Domain.Services
{
    public class SampleService : ISampleService
    {
        public SampleService()
        {

        }

        public string GetDummyValue()
        {
            return "dummy value";
        }
    }
}
