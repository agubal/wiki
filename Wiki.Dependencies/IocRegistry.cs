using System.Configuration;
using MediatR;
using StructureMap;
using Wiki.Data;
using Wiki.Data.Context;
using Wiki.Data.Domain.Pages.Commands;
using Wiki.Services.Events;
using Wiki.Services.Pages;

namespace Wiki.Dependencies
{
    public class IocRegistry : Registry
    {
        public IocRegistry()
        {
            Register();
        }

        private void Register()
        {
            //DAL:
            Scan(scanner =>
            {
                scanner.AssemblyContainingType<Insert>();
                scanner.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncRequestHandler<,>));
                scanner.ConnectImplementationsToTypesClosing(typeof(INotificationHandler<>));
                scanner.ConnectImplementationsToTypesClosing(typeof(IAsyncNotificationHandler<>));
            });
            For<SingleInstanceFactory>().Use<SingleInstanceFactory>(ctx => t => ctx.GetInstance(t));
            For<MultiInstanceFactory>().Use<MultiInstanceFactory>(ctx => t => ctx.GetAllInstances(t));
            For<IMediator>().Use<Mediator>();
            For<IDapperContext>().Add<DapperContext>().Ctor<string>("connectionString").Is(ConfigurationManager.ConnectionStrings["WikiContext"].ConnectionString);
            For<IRepository>().Use<Repository>();

            //BLL:
            For<IEventService>().Use<EventService>();
            For<IPageService>().Use<PageService>();
        }
    }
}
