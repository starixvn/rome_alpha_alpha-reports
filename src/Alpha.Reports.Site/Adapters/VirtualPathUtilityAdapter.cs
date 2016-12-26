namespace Alpha.Reports.Site.Adapters
{
    using System;
    using System.Web;

    public static class VirtualPathUtilityAdapter
    {
        [ThreadStatic]
        private static Func<string, string> funcToAbsolute;

        public static Func<string, string> FuncToAbsolute
        {
            get { return funcToAbsolute ?? VirtualPathUtility.ToAbsolute; }
            set { funcToAbsolute = value; }
        }
    }
}