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
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using RemotingRef;
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
            yield return typeof(BaseReportController).Assembly;
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<CacheRefService, CacheRefServiceImpl>();
            container.RegisterPerWebRequest<IUserSession, UserSessionExt>();

            container.RegisterPerWebRequest<ReportDateService, ReportDateServiceImpl>();
            container.RegisterPerWebRequest<ProductService, ProductServiceImpl>();

            container.RegisterPerWebRequest<WinLossDetailService, WinLossDetailServiceImpl>();
            container.RegisterPerWebRequest<WinLossDetailExcelExporter, WinLossDetailExcelExporterImpl>();
            container.RegisterPerWebRequest<SpreadSheetEngine, SpreadSheetEngineImpl>();
            container.RegisterPerWebRequest<Workbook, NpoiWorkbook>();
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterPerWebRequest<IDynamicRepository, DynamicRepository>();
            container.RegisterPerWebRequest<WinLossDetailDataAccess, WinLossDetailDataAccessImpl>();
        }
    }
}