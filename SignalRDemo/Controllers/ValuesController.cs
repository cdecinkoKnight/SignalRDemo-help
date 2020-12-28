using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRDemo.Domain.Services;
using SignalRDemo.Hubs;
using System;
using System.Web.Http;

namespace SignalRDemo.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ISampleService _sampleService;
        private IConnectionManager _connectionManager;

        public ValuesController(ISampleService sampleService, HubConfiguration hubConfiguration)
        {
            _sampleService = sampleService;
            _connectionManager = hubConfiguration.Resolver.Resolve<IConnectionManager>();
        }


        // GET api/values
        [HttpGet]
        public string Get()
        {
            var context = _connectionManager.GetHubContext<MessageHub>();

            context.Clients.All.showControllerMessageOnPage("From controller: " + DateTime.Now);

            return _sampleService.GetDummyValue();
        }
    }
}
