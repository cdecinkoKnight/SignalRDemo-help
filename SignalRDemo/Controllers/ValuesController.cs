using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRDemo.Domain.Services;
using SignalRDemo.Hubs;
using System.Web.Http;

namespace SignalRDemo.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ISampleService _sampleService;
        private IConnectionManager _connectionManager;

        public ValuesController(ISampleService sampleService, IConnectionManager connectionManager)
        {
            _sampleService = sampleService;
            _connectionManager = connectionManager;
        }


        // GET api/values
        [HttpGet]
        public string Get()
        {
            var context = _connectionManager.GetHubContext<MessageHub>();
            context.Clients.All.showMessageOnPage("From controller");

            return _sampleService.GetDummyValue();
        }
    }
}
