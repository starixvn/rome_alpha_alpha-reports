namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Web;

    public static class HttpAdapter
    {
        private static bool? hasContext;

        private static Func<string, string> applyAppPathModifierFunc;

        private static string appDomainAppVirtualPath;

        public static bool HasContext
        {
            get { return hasContext ?? (HttpContext.Current != null && HttpContext.Current.CurrentHandler != null); }
            set { hasContext = value; }
        }

        public static Func<string, string> ApplyAppPathModifierFunc
        {
            get { return applyAppPathModifierFunc ?? HttpContext.Current.Response.ApplyAppPathModifier; }
            set { applyAppPathModifierFunc = value; }
        }

        public static string AppDomainAppVirtualPath
        {
            get { return appDomainAppVirtualPath ?? HttpRuntime.AppDomainAppVirtualPath; }
            set { appDomainAppVirtualPath = value; }
        }
    }
}