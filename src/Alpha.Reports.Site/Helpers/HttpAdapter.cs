namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Web;

    public static class HttpAdapter
    {
        [ThreadStatic]
        private static bool? hasContext;

        [ThreadStatic]
        private static Func<string, string> applyAppPathModifierFunc;

        [ThreadStatic]
        private static Func<string, string> serverMapPathFunc;

        [ThreadStatic]
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

        public static Func<string, string> ServerMapPathFunc
        {
            get { return serverMapPathFunc ?? HttpContext.Current.Server.MapPath; }
            set { serverMapPathFunc = value; }
        }
    }
}