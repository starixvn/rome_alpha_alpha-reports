namespace Alpha.Reports.Site
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Logging;
    using Alpha.NetThrottling.Mvc;
    using Alpha.NetThrottling.Utils;
    using Alpha.Reports.Site.Constants;
    using App_Start;
    using Auth.Configurations;
    using Configurations;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Data.CrossCutting.WrappingException;
    using Fanex.Data.OldConnectionStringProvider;
    using Fanex.Logging.Extensions.Database;
    using Fanex.Logging.Extensions.Web;
    using Fanex.RemotingRef;
    using Helpers;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using Sunplus.Agent.Model.Entities;
    using Web;
    using Web.Filters;
    using Web.Helpers;

    public class MvcApplication : HttpApplication
    {
        private static string CustomerIp()
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            var ips = string.IsNullOrWhiteSpace(ip) || ip.Equals("::1") ?
                new[] { "127.0.0.1" } : ip.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            return ips[0].Trim();
        }

        protected void Application_AcquireRequestState()
        {
            var session = UserSession.Current;
            if (session.IsLogged && Request.IsAuthenticated && Request.Url.PathAndQuery.IndexOf("/Auth", StringComparison.OrdinalIgnoreCase) == -1)
            {
                var loginCredential = session.GetProperty<string>(SessionKey.LoginIP_Browser);
                var requestCredential = CustomerIp() + "_" + Request.Browser.Version;

                if (!loginCredential.Equals(requestCredential, StringComparison.OrdinalIgnoreCase))
                {
                    Response.Redirect($"~/{ConfigurationManager.AppSettings["Report:Routing"]}Auth", true);
                }
            }
        }

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            DependencyConfig.Init();

            DbSettingProviderManager
                .StartNewSession()
                .Use(new OldConnectionStringProvider())
                .UseDefaultDbSettingProvider(resourcePath: SiteSettings.StoredProcedurePath, enableWatching: true)
                .Use((settingKey, commandTimeout) => new WrappingExceptionObjectDb(settingKey, commandTimeout))
                .Run();

            ShareSession.Init();

            LoggingConfig.Init(getDbExceptionInfo: DatabaseInfoGetters.GetDbExceptionInfo);

            GlobalFilters.Filters.Add(new NetThrottlingFilterAttribute(isUsedRedis: false));

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new HttpContextHelper());
            GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));

            GlobalFilters.Filters.Add(new NoCacheFilter());
            GlobalFilters.Filters.Add(new PartialUMExceptionFilter());

            SiteSettings.Verify();

            ModelBinders.Binders.DefaultBinder = new QueryEntityModelBinder();

            StaticResources.Init(GetBuildVersion());

            RefDataTypeContainer
                .ConnectTo(new Uri(SiteSettings.RemoteRefURL))
                .FromApplication("Alpha.Reports.Site");
        }

        protected void Application_Error()
        {
            if (Request.IsLocal)
            {
                return;
            }

            Exception ex = Server.GetLastError();
            HttpException httpException = ex as HttpException;
            if (httpException != null && httpException.GetHttpCode() == HttpStatusCodes.TooManyRequests)
            {
                return;
            }

            var currentCustomer = UserSession.Current;
            if (ex is FormatException &&
                Request.Params.AllKeys.Any(key => AuthorizationSection.Instance.CustIdQueryStringParamNames.Any(cf => string.Equals(key, cf, StringComparison.OrdinalIgnoreCase))))
            {
                return;
            }

            ExceptionHandler.Handle(ex,
                new Dictionary<string, object>
                {
                    ["CustId"] = currentCustomer?.CustId,
                    ["CustName"] = currentCustomer?.UserName,
                    ["SubAccountName"] = currentCustomer?.SubUserName,
                    ["AdminName"] = currentCustomer?.AdminName
                });
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}