using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using NHibernate;
using OracleFiddler.Core;
using OracleFiddler.WebUi.Infrastructure.Profiling;
using StackExchange.Profiling;

namespace OracleFiddler.WebUi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            BeginRequest += (sender, args) =>
            {
                if (Request.IsLocal)
                {
                    MiniProfiler.Start();
                }
            };

            EndRequest += (sender, args) => MiniProfiler.Stop();
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new CoreDependencies<ProfiledOracleClientDriver>());

            builder
                .Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .InstancePerHttpRequest();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }

        private void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new
                {
                    controller = "SchemaExplorer",
                    action = "Schemas",
                    id = UrlParameter.Optional
                });
        }

        private void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}