namespace Alpha.Reports.Site.App_Start
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Mvc;
    using Configurations;
    using Controllers;
    using DataAccesses;
    using Fanex.Data.Repository;
    using Handlers.CommissionByBettype;
    using Handlers.Consolidation;
    using Handlers.MatchWinLossDetail;
    using Handlers.ProgressiveContribution;
    using Handlers.Statement;
    using Handlers.WinLossByProduct;
    using MediatR;
    using Models;
    using ONEbook.SpreadSheets;
    using ONEbook.SpreadSheets.NPOI;
    using RemotingRef;
    using Reports.Models;
    using Services;
    using Services.Outstanding;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using StaticData.Repositories;
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

            RegisterStaticInstances(container);
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
            container.RegisterSingleton<MatchWinLossService, MatchWinLossServiceImpl>();
            container.RegisterSingleton<OutstandingService, OutstandingServiceImpl>();
            container.RegisterSingleton<WinLossByProductService, WinLossByProductServiceImpl>();
            container.RegisterSingleton<WinLossAnalysisService, WinLossAnalysisServiceImpl>();
            container.RegisterSingleton<WinLossAnalysisChartService, WinLossAnalysisChartServiceImpl>();
            container.RegisterSingleton<PlayForFunService, PlayForFunServiceImpl>();
            container.RegisterSingleton<CommissionByBettypeService, CommissionByBettypeServiceImpl>();

            container.RegisterSingleton<MatchWinLossDetailService, MatchWinLossDetailServiceImpl>();
            container.RegisterSingleton<UserPermissionService, UserPermissionServiceImpl>();
            container.RegisterSingleton<ConsolidationService, ConsolidationServiceImpl>();

            container.Register<ProductService>(
                () => new ProductServiceImpl(
                    container.GetInstance<IDynamicRepository>(),
                    UserPermission.HasPermissionOnProduct,
                    new CacheRefServiceImpl().GetProductName),
                Lifestyle.Singleton);

            container.Register(typeof(Workbook<>), typeof(NpoiWorkbook<>), Lifestyle.Scoped);

            container.Register<CommissionByBettypeWorkbook, CommissionByBettypeWorkbookImpl>();
            container.RegisterSingleton<LeagueService, LeagueServiceImpl>();
            container.RegisterSingleton<WinLossByProductWorkbook, WinLossByProductWorkbookImpl>();
            container.RegisterSingleton<MatchWinLossDetailWorkbook, MatchWinLossDetailWorkbookImpl>();

            container.RegisterSingleton<StatementService, StatementServiceImpl>();
            container.RegisterSingleton<MemberStatementWorkbook, MemberStatementWorkbookImpl>();
            container.RegisterSingleton<StatementWorkbook, StatementWorkbookImpl>();

            container.RegisterSingleton<ConsolidationByDateWorkbook, WinLossByDateWorkbookImpl>();
            container.RegisterSingleton<ConsolidationByProductWorkbook, ConsolidationByProductWorkbookImpl>();

            container.RegisterSingleton<ProgressiveContributionService, ProgressiveContributionServiceImpl>();
            container.RegisterSingleton<ProgressiveContributionWorkbook, ProgressiveContributionWorkbookImpl>();
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterSingleton<WinLossDetailDataAccess, WinLossDetailDataAccessImpl>();
            container.RegisterSingleton<WinLossDataAccess, WinLossDataAccessImpl>();
            container.RegisterSingleton<WinLossAnalysisDataAccess, WinLossAnalysisDataAccessImpl>();
            container.RegisterSingleton<MatchWinLossDetailDataAccess, MatchWinLossDetailDataAccessImpl>();
            container.RegisterSingleton<StatementDataAccess, StatementDataAccessImpl>();
            container.RegisterSingleton<WinLossByProductDataAccess, WinLossByProductDataAccessImpl>();
            container.RegisterSingleton<ProgressiveContributionDataAccess, ProgressiveContributionDataAccessImpl>();

            if (SiteSettings.EnabledStaticData)
            {
                container.Register<IDynamicRepository, ReportExcelRepository>(Lifestyle.Singleton);
            }
            else
            {
                container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Singleton);
            }
        }

        private static void RegisterStaticInstances(Container container)
        {
            ReportDateServiceImpl.Instance = container.GetInstance<ReportDateService>();
        }
    }
}