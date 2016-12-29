namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web.Configuration;

    public static class BaseStaticResources
    {
        [ThreadStatic]
        private static string minText;

        [ThreadStatic]
        private static string staticResourcesVersion;

        public static string MinText
        {
            get
            {
                if (minText == null)
                {
                    minText = IsDebugMode() ? string.Empty : ".min";
                }

                return minText;
            }

            set
            {
                minText = value;
            }
        }

        public static string StaticResourcesVersion
        {
            get
            {
                if (staticResourcesVersion == null)
                {
                    staticResourcesVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
                }

                return staticResourcesVersion;
            }

            set
            {
                staticResourcesVersion = value;
            }
        }

        private static bool IsDebugMode()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

            return compilationSection.Debug;
        }
    }
}