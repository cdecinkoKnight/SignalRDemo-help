using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using SignalRDemo;
using SignalRDemo.Domain.Hubs;
using SignalRDemo.Domain.Services;
using SignalRDemo.Hubs;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace SignalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            //config.Routes.IgnoreRoute("signalr", "signalr/{*pathInfo}");
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "v1/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            //builder.RegisterType<AutofacDependencyResolver>().As<IDependencyResolver>().SingleInstance();
            //builder.RegisterType<ConnectionManager>().As<IConnectionManager>();
            builder.RegisterType<SampleService>().As<ISampleService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageBroker>().As<IMessageBroker>();

            HubConfiguration hubConfig = new HubConfiguration();
            builder.RegisterInstance(hubConfig);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);
            app.UseCors(CorsOptions.AllowAll);
            SwaggerConfig.Register(config);
            app.UseWebApi(config);


            app.Map("/signalr", map =>
            {
                map.UseCors(SignalrCorsOptions.Value);
                map.UseAutofacMiddleware(container);
                hubConfig.Resolver = new AutofacDependencyResolver(container);
                hubConfig.EnableDetailedErrors = true;
                hubConfig.EnableJavaScriptProxies = true;
                map.RunSignalR(hubConfig);
            });

            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            //FilterConfig.RegisterGlobalFilters(System.Web.Mvc.GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);

        }

        private static readonly Lazy<CorsOptions> SignalrCorsOptions = new Lazy<CorsOptions>(() =>
        {
            return new CorsOptions
            {
                PolicyProvider = new CorsPolicyProvider
                {
                    PolicyResolver = context =>
                    {
                        var policy = new CorsPolicy();
                        policy.AllowAnyOrigin = true;
                        policy.Origins.Add("http://localhost:44386");
                        policy.AllowAnyMethod = true;
                        policy.AllowAnyHeader = true;
                        policy.SupportsCredentials = true;
                        return Task.FromResult(policy);
                    }
                }
            };
        });
    }
}