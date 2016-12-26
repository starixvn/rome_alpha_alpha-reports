namespace Alpha.Reports.Site.Adapters
{
    using System;
    using System.Web;

    public static class HttpAdapter
    {
        [ThreadStatic]
        private static bool? hasContext;

        [ThreadStatic]
        private static Func<string, string> funcApplyAppPathModifier;

        [ThreadStatic]
        private static Func<string, string> funcServerMapPath;

        [ThreadStatic]
        private static string appDomainAppVirtualPath;

        public static bool HasContext
        {
            get { return hasContext ?? (HttpContext.Current != null && HttpContext.Current.CurrentHandler != null); }
            set { hasContext = value; }
        }

        public static Func<string, string> FuncApplyAppPathModifier
        {
            get { return funcApplyAppPathModifier ?? HttpContext.Current.Response.ApplyAppPathModifier; }
            set { funcApplyAppPathModifier = value; }
        }

        public static string AppDomainAppVirtualPath
        {
            get { return appDomainAppVirtualPath ?? HttpRuntime.AppDomainAppVirtualPath; }
            set { appDomainAppVirtualPath = value; }
        }

        public static Func<string, string> FuncServerMapPath
        {
            get { return funcServerMapPath ?? HttpContext.Current.Server.MapPath; }
            set { funcServerMapPath = value; }
        }
    }
}