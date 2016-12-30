namespace Alpha.Reports.Site
{
    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using App_GlobalResources;
    using App_Start;
    using Auth.Filters;
    using Configurations;
    using Fanex.Data;
    using ONEbook.UM.Filters;
    using Requests;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using Web;

    public class MvcApplication : HttpApplication
    {
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
                .UseDefaultConnectionStringProvider()
                .UseDefaultDbSettingProvider(
                    resourcePath: SiteSettings.StoreProcedurePath,
                    enableWatching: true)
                .Run();

            ShareSession.Init();

            GlobalFilters.Filters.Add(new CheckUMFilter());
            GlobalFilters.Filters.Add(new CheckBelongingFilter());

            SiteSettings.Verify();

            ModelBinders.Binders.DefaultBinder = new UserModelBinder();

            Logs.WriteInfo(Resources.AppStart_Message);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError().GetBaseException();
            var isIgnoreError = Request.IsLocal ||
                ex is HttpException ||
                ex.Message.Contains("Extend Token Error UserID");

            if (!isIgnoreError)
            {
                Logs.FilterInfoWarnOrError(ex, UserSession.Current.CustId);
            }
        }

        protected void Application_End()
        {
            HttpRuntime runtime = (HttpRuntime)typeof(HttpRuntime).InvokeMember(
                "_theRuntime",
                BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField,
                null,
                null,
                null);

            if (runtime == null)
            {
                return;
            }

            string shutDownMessage = (string)runtime.GetType().InvokeMember(
                "_shutDownMessage",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                null,
                runtime,
                null);

            string shutDownStack = (string)runtime.GetType().InvokeMember(
                "_shutDownStack",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField,
                null,
                runtime,
                null);

            var logMessage = string.Format(
                "\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}",
                shutDownMessage,
                shutDownStack);

            Logs.WriteInfo(logMessage);
        }
    }
}