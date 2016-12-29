namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using Fanex.Common.Helper;

    public static class AppPathHelper
    {
        public static string GetAppPath(string appName, string virtualPath)
        {
            Check.Argument.IsNotEmpty(appName, nameof(appName));
            Check.Argument.IsNotEmpty(virtualPath, nameof(virtualPath));

            if (HttpContext.Current == null || HttpContext.Current.CurrentHandler == null)
            {
                return string.Empty;
            }

            var appPath = HttpContext.Current.Response.ApplyAppPathModifier(virtualPath);
            appPath = Regex.Replace(appPath, HttpRuntime.AppDomainAppVirtualPath, appName, RegexOptions.IgnoreCase);

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