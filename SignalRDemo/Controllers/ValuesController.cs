using Microsoft.AspNet.SignalR.Infrastructure;
using SignalRDemo.Hubs;
using System.Collections.Generic;
using System.Web.Http;

namespace SignalRDemo.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly IConnectionManager _connectionManager;

        public ValuesController(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var context = _connectionManager.GetHubContext<MessageHub>();
            context.Clients.All.showMessageOnPage("From controller");

            return new string[] { "value1", "value2" };
        }
    }
}
