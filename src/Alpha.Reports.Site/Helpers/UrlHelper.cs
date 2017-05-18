namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Configurations;
    using Enumerations;
    using Reports.Models;
    using Web.Helpers;

    public static class UrlHelper
    {
        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> addQueryStringParamsFunc;

        [ThreadStatic]
        private static Func<string, IDictionary<string, object>, string> replaceOrAddQueryStringParamsFunc;

        [ThreadStatic]
        private static Func<string, string> mapExMainPath;

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
            get { return mapExMainPath ?? MicroSites.OldMainUrl; }

            internal set { mapExMainPath = value; }
        }

        public static Func<string, string, string> GetSiteUrl { get; internal set; } = GetUrl;

        internal static string AddQueryStringParams(string url, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return url;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{url}?{queryString}";
        }

        internal static string ReplaceOrAddQueryStringParams(string url, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return url;
            }

            if (!url.Contains("?"))
            {
                return AddQueryStringParams(url, routeValues);
            }

            foreach (var routeValue in routeValues)
            {
                url = ReplaceOrAddQueryStringParam(url, routeValue.Key, routeValue.Value);
            }

            return url;
        }

        internal static string ReplaceOrAddQueryStringParam(string url, string paramName, object paramValue)
        {
            if (string.IsNullOrWhiteSpace(paramName) || paramValue == null)
            {
                return url;
            }

            var regexParamName = @"([?&]" + paramName + ")=[^?&]+";
            if (Regex.IsMatch(url, regexParamName, RegexOptions.IgnoreCase))
            {
                return Regex.Replace(url, regexParamName, "$1=" + paramValue, RegexOptions.IgnoreCase);
            }

            return url + $"&{paramName}={paramValue}";
        }

        internal static string GetUrl(string action, string currentQuery)
        {
            var relativePath = action + currentQuery;
            var url = AppPathHelper.GetAppPath(SiteSettings.ReportsAppPath, relativePath);

            return url;
        }
    }
}