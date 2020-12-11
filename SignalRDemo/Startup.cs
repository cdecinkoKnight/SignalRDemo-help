using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using SignalRDemo;
using System.Reflection;

[assembly: OwinStartup(typeof(Startup))]

namespace SignalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            var config = new HubConfiguration();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterType<DefaultAssemblyLocator>().As<IAssemblyLocator>();
            builder.RegisterType<AutofacDependencyResolver>().As<IDependencyResolver>().SingleInstance();
            builder.RegisterType<ConnectionManager>().As<IConnectionManager>().SingleInstance();

            var container = builder.Build();
            config.Resolver = new AutofacDependencyResolver(container);

            app.UseAutofacMiddleware(container);
            app.MapSignalR("/signalr", config);
        }
    }
}