using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
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
            app.UseCors(CorsOptions.AllowAll);
            //app.UseAutofacWebApi(config);
            //app.UseWebApi(config);

            app.Map("/signalr", map =>
            {
                map.UseCors(SignalrCorsOptions.Value);
                map.UseAutofacMiddleware(container);

                var hubConfiguration = new HubConfiguration
                {
                    Resolver = new AutofacDependencyResolver(container),
                };
                hubConfiguration.EnableDetailedErrors = true;
                hubConfiguration.EnableJavaScriptProxies = true;
                //hubConfiguration.EnableJSONP = true;

                //var hubPipeline = hubConfiguration.Resolver.Resolve<IHubPipeline>();
                //hubPipeline.AddModule(new LoggingPipelineModule());

                map.RunSignalR(hubConfiguration);
            });
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