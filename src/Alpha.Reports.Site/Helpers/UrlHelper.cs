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

        private static string AddQueryStringParams(string url, IDictionary<string, object> routeValues)
        {
            var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            return $"{url}?{queryString}";
        }

        private static string ReplaceOrAddQueryStringParams(string url, IDictionary<string, object> routeValues)
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

        private static string ReplaceOrAddQueryStringParam(string url, string paramName, object paramValue)
        {
            if (string.IsNullOrWhiteSpace(paramName) || paramValue == null)
            {
                return url;
            }

            if (url.Contains("?"))
            {
                var regexParamName = @"([?&]" + paramName + ")=[^?&]+";
                if (Regex.IsMatch(url, regexParamName))
                {
                    return Regex.Replace(url, regexParamName, "$1=" + paramValue);
                }

                return url + $"&{paramName}={paramValue}";
            }

            return url + $"?{paramName}={paramValue}";
        }
    }
}