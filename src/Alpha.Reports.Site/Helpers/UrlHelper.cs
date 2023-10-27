namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using Alpha.Reports.Configurations;
    using Web.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, string> mapExMainPath;

        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> addQueryStringParamsFunc;

        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> replaceOrAddQueryStringParamsFunc;

        public static Func<string, IDictionary<string, object>, string> AddQueryStringParamsFunc
        {
            get
            {
                return addQueryStringParamsFunc ?? AddQueryStringParams;
            }

            internal set
            {
                addQueryStringParamsFunc = value;
            }
        }

        public static Func<string, IDictionary<string, object>, string> ReplaceOrAddQueryStringParamsFunc
        {
            get
            {
                return replaceOrAddQueryStringParamsFunc ?? ReplaceOrAddQueryStringParams;
            }

            internal set
            {
                replaceOrAddQueryStringParamsFunc = value;
            }
        }

        public static Func<string, string> MapExMainPath
        {
            get
            {
                return mapExMainPath ?? MicroSites.OldMainUrl;
            }

            internal set
            {
                mapExMainPath = value;
            }
        }

        public static Func<string, string, string> GetSiteUrl { get; internal set; } = GetLink;

        private static Func<string, string> mapBetListPath;
        public static Func<string, string> MapBetListPath
        {
            get
            {
                mapBetListPath = mapBetListPath ?? GetBetListPath;

                return mapBetListPath;
            }

            internal set
            {
                mapBetListPath = value;
            }
        }

        internal static Func<string, string> RemapBetListPath
        {
            get
            {
                var context = HttpContext.Current;
                if (SiteSettings.IsStandaloneReport && context.Session.IsCookieless)
                {
                    var betlistDomain = $"//{context.Request.Headers["Host"]}";
                    var betlistSessionId = context.Session.SessionID;
                    var betlistAppPath = "/site-betlists";

                    return new Func<string, string>((virtualPath) => $"{betlistDomain}{betlistAppPath}/(S({betlistSessionId})){virtualPath?.Replace("~", string.Empty)}");
                }

                return MicroSites.BetListsUrl;
            }
        }

        internal static string GetBetListPath(string virtualPath = "~/WinLoss/Index")
            => GetAppPath(virtualPath, RemapBetListPath, SiteSettings.BetListAppPath);

        internal static string AddQueryStringParams(string link, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return link;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{link}?{queryString}";
        }

        internal static string ReplaceOrAddQueryStringParams(string link, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return link;
            }

            if (!link.Contains("?"))
            {
                return AddQueryStringParams(link, routeValues);
            }

            foreach (var routeValue in routeValues)
            {
                link = ReplaceOrAddQueryStringParam(link, routeValue.Key, routeValue.Value);
            }

            return link;
        }

        internal static string ReplaceOrAddQueryStringParam(string link, string paramName, object paramValue)
        {
            if (string.IsNullOrWhiteSpace(paramName) || paramValue == null)
            {
                return link;
            }

            var regexParamName = @"([?&]" + paramName + ")=[^?&]+";
            if (Regex.IsMatch(link, regexParamName, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(link, regexParamName, "$1=" + paramValue, RegexOptions.IgnoreCase);
            }

            return link + $"&{paramName}={paramValue}";
        }

        internal static string GetLink(string action, string currentQuery)
        {
            if (SiteSettings.IsStandaloneReport && HttpContext.Current.Session.IsCookieless)
            {
                return MicroSites.ReportsUrl(action + currentQuery).Replace("/site-reports", string.Empty);
            }

            return MicroSites.ReportsUrl(action + currentQuery);
        }

        private static Func<string, string> mapMessagePath;
        public static Func<string, string> MapMessagePath
        {
            get
            {
                mapMessagePath = mapMessagePath ?? GetMessagePath;

                return mapMessagePath;
            }
        }

        internal static string GetMessagePath(string virtualPath)
        {
            var context = HttpContext.Current;
            if (SiteSettings.IsStandaloneReport && context.Session.IsCookieless)
            {
                var mainDomain = $"//{context.Request.Headers["Host"]}";
                var mainSessionId = context.Session.SessionID;

                return $"{mainDomain}/site-messages/(S({mainSessionId})){virtualPath?.Replace("~", string.Empty)}";
            }

            return GetAppPath(virtualPath, MicroSites.MessageUrl, AlphaPlus.Configurations.AppSettings.MessageAppPath);
        }

        internal static string GetAppPath(string virtualPath, Func<string, string> getAppPath, string appPath)
        {
            return getAppPath(virtualPath);
        }
    }
}