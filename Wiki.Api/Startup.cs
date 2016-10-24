using System.Web.Http;
using Microsoft.Owin.Cors;
using Owin;
using StructureMap;
using Wiki.Api.Filters;
using Wiki.Dependencies;

namespace Wiki.Api
{
    public class Startup
    {
        private readonly IContainer _container;

        public Startup()
        {
            _container = IoC.Initialize();
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration
            {
                DependencyResolver = new DependencyResolution.StructureMapWebApiDependencyResolver(_container),
                Filters = { new WikiErrorHandler() }
            };
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }
}