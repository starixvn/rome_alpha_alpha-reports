namespace Alpha.Reports.Site.Helpers
{
    using System;
    using Newtonsoft.Json;

    public static class JsonHelper
    {
        public static Func<object, string> ToJsonFunc { get; internal set; } = JsonConvert.SerializeObject;
    }
}