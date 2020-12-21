﻿using Autofac;
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
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);
            builder.RegisterHubs(Assembly.GetExecutingAssembly());

            builder.RegisterType<AutofacDependencyResolver>().As<IDependencyResolver>().SingleInstance();
            builder.RegisterType<ConnectionManager>().As<IConnectionManager>().SingleInstance();
            builder.RegisterType<SampleService>().As<ISampleService>().InstancePerLifetimeScope();
            builder.RegisterType<MessageBroker>().As<IMessageBroker>();

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

                var hubConfiguration = new HubConfiguration
                {
                    Resolver = new AutofacDependencyResolver(container),
                };
                hubConfiguration.EnableDetailedErrors = true;
                hubConfiguration.EnableJavaScriptProxies = true;

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