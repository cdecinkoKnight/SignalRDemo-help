using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using SignalRDemo;
using SignalRDemo.Domain.Hubs;
using SignalRDemo.Domain.Services;
using SignalRDemo.Hubs;
using System.Reflection;
using System.Web.Http;

[assembly: OwinStartup(typeof(Startup))]

namespace SignalRDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterType<AutofacDependencyResolver>().As<IDependencyResolver>().SingleInstance();
            builder.RegisterType<ConnectionManager>().As<IConnectionManager>().SingleInstance();
            builder.RegisterType<SampleService>().As<ISampleService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageBroker>().As<IMessageBroker>();

            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Register the Autofac middleware FIRST, then the Autofac Web API middleware,
            // and finally the standard Web API middleware.
            app.UseAutofacMiddleware(container);
            //app.UseAutofacWebApi(config);
            //app.UseWebApi(config);
        }
    }
}