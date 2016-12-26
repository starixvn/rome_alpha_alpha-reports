namespace Alpha.Reports.Site.Adapters
{
    using System;
    using System.Web;

    public static class WebOptimizationAdapter
    {
        [ThreadStatic]
        private static Func<string[], IHtmlString> funcRenderScripts;

        [ThreadStatic]
        private static Func<string[], IHtmlString> funcRenderStyles;

        public static Func<string[], IHtmlString> FuncRenderScripts
        {
            get { return funcRenderScripts ?? System.Web.Optimization.Scripts.Render; }
            set { funcRenderScripts = value; }
        }

        public static Func<string[], IHtmlString> FuncRenderStyles
        {
            get { return funcRenderStyles ?? System.Web.Optimization.Styles.Render; }
            set { funcRenderStyles = value; }
        }
    }
}