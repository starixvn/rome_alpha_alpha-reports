namespace Alpha.Reports.Site.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Web.Helpers;

    public static class UrlHelper
    {
        public static Func<string, IDictionary<string, object>, string> AddQueryStringParamsFunc { get; internal set; } = AddQueryStringParams;

        public static Func<string, IDictionary<string, object>, string> ReplaceOrAddQueryStringParamsFunc { get; internal set; } = ReplaceOrAddQueryStringParams;

        public static Func<string, string> MapExMainPath { get; internal set; } = MicroSites.OldMainUrl;

        internal static string AddQueryStringParams(string url, IDictionary<string, object> routeValues)
        {
            if (!routeValues.Any())
            {
                return url;
            }

            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{url}?{queryString}".ToLower();
        }

        internal static string ReplaceOrAddQueryStringParams(string url, IDictionary<string, object> routeValues)
        {
            if (routeValues.Any())
            {
                if (!url.Contains("?"))
                {
                    return AddQueryStringParams(url, routeValues);
                }

                foreach (var routeValue in routeValues)
                {
                    url = ReplaceOrAddQueryStringParam(url, routeValue.Key, routeValue.Value);
                }
            }

            return url.ToLower();
        }

        internal static string ReplaceOrAddQueryStringParam(string url, string paramName, object paramValue)
        {
            if (string.IsNullOrWhiteSpace(paramName) || paramValue == null)
            {
                return url;
            }

            var regexParamName = @"([?&]" + paramName + ")=[^?&]+";
            if (Regex.IsMatch(url, regexParamName))
            {
                return Regex.Replace(url, regexParamName, "$1=" + paramValue);
            }

            return url + $"&{paramName}={paramValue}";
        }
    }
}