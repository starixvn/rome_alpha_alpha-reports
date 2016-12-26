namespace Alpha.Reports.Site.Adapters
{
    using System;
    using System.Diagnostics;
    using System.Reflection;

    public static class AssemblyAdapter
    {
        [ThreadStatic]
        private static string version;

        public static string Version
        {
            get { return version ?? FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion; }
            set { version = value; }
        }
    }
}