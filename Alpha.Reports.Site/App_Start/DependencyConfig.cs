namespace Alpha.Reports.Site.App_Start
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Controllers;
    using DataAccesses;
    using DataAccesses.Impl;
    using Excel.Services;
    using Excel.Services.Impl;
    using Fanex.Data.Repository;
    using MediatR;
    using RemotingRef;
    using Reports;
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;

    public static class DependencyConfig
    {
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);

            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(BaseController).Assembly;
        }

        private static void RegisterServices(Container container)
        {
            container.Register<ApplicationContext, ReportApplicationContext>(Lifestyle.Singleton);
            container.Register<IUserSession, UserSessionExt>(Lifestyle.Scoped);
            container.Register<CacheRefService, CacheRefServiceImpl>(Lifestyle.Transient);
            container.Register<ReportDateService, ReportDateServiceImpl>(Lifestyle.Transient);
            container.Register<ProductService, ProductServiceImpl>(Lifestyle.Transient);
            container.Register<WinLossDetailService, WinLossDetailServiceImpl>(Lifestyle.Transient);

            container.Register<WinLossDetailExcelExporter, WinLossDetailExcelExporterImpl>(Lifestyle.Transient);
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Transient);
            container.Register<WinLossDetailDataAccess, WinLossDetailDataAccessImpl>(Lifestyle.Transient);
        }
    }
}