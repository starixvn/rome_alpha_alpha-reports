namespace Alpha.Reports.Site.Helpers.Impl.StaticResources
{
    using System.Configuration;
    using System.Diagnostics;
    using System.Reflection;
    using System.Web.Configuration;

    public static class BaseStaticResources
    {
        public static readonly string MinText = IsDebugMode() ? string.Empty : ".min";

        public static readonly string StaticResourcesVersion = GetBuildVersion();

        private static bool IsDebugMode()
        {
            var compilationSection = (CompilationSection)ConfigurationManager.GetSection(@"system.web/compilation");

            return compilationSection.Debug;
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return versionInfo.FileVersion;
        }
    }
}