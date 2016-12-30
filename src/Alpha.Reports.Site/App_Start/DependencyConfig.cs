namespace Alpha.Reports.Site.App_Start
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Controllers;
    using DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Data.Repository;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using RemotingRef;
    using Reports.Models;
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

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
            yield return typeof(ReportBaseController).Assembly;
        }

        private static void RegisterServices(Container container)
        {
            container.RegisterSingleton<CacheRefService, CacheRefServiceImpl>();
            container.RegisterSingleton<IUserSession, UserSessionExt>();
            container.RegisterSingleton<IUser, User>();
            container.RegisterSingleton<ReportDateService, ReportDateServiceImpl>();           

            container.RegisterSingleton<WinLossDetailService, WinLossDetailServiceImpl>();
            container.RegisterSingleton<WinLossService, WinLossServiceImpl>();

            container.Register<ProductService>(
                () => new ProductServiceImpl(
                    container.GetInstance<IDynamicRepository>(),
                    UserPermission.HasPermissionOnProduct,
                    new CacheRefServiceImpl().GetProductName), 
                Lifestyle.Singleton);

            container.Register(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped);           
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterSingleton<IDynamicRepository, DynamicRepository>();
            container.RegisterSingleton<WinLossDetailDataAccess, WinLossDetailDataAccessImpl>();
            container.RegisterSingleton<WinLossDataAccess, WinLossDataAccessImpl>();
        }
    }
}