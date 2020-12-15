using System.Web.Http;
using System.Web.Http.Cors;

namespace SignalRDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://localhost:44386", "Content-Type", "GET, PATCH, POST, OPTIONS", "*") { SupportsCredentials = true };
            config.EnableCors(cors);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
