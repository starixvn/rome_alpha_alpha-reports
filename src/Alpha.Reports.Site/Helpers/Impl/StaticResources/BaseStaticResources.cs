namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System;
    using System.Web.Configuration;
    using Adapters;

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
                    staticResourcesVersion = AssemblyAdapter.Version;
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
            var compilationSection = (CompilationSection)ConfigurationManagerAdapter.GetSection(@"system.web/compilation");

            return compilationSection.Debug;
        }
    }
}