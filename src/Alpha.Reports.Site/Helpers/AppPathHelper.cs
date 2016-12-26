namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Text.RegularExpressions;
    using Adapters;
    using Fanex.Common.Helper;

    public static class AppPathHelper
    {
        public static string GetAppPath(string appName, string virtualPath)
        {
            Check.Argument.IsNotEmpty(appName, nameof(appName));
            Check.Argument.IsNotEmpty(virtualPath, nameof(virtualPath));

            if (!HttpAdapter.HasContext)
            {
                return string.Empty;
            }

            var appPath = HttpAdapter.FuncApplyAppPathModifier(virtualPath);
            appPath = Regex.Replace(appPath, HttpAdapter.AppDomainAppVirtualPath, appName, RegexOptions.IgnoreCase);

            return appPath;
        }

        public static string GetRelativePath(string virtualPath, string absolutePath)
        {
            if (string.IsNullOrWhiteSpace(virtualPath) || string.IsNullOrWhiteSpace(absolutePath))
            {
                return virtualPath;
            }

            if (absolutePath.StartsWith(virtualPath, StringComparison.OrdinalIgnoreCase))
            {
                return absolutePath.Substring(virtualPath.Length + 1);
            }

            return absolutePath;
        }
    }
}