namespace Alpha.Reports.SpamBlocker.Web
{
    using Alpha.Reports.SpamBlocker.Models;
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class EnableThrottlingAttribute : ActionFilterAttribute
    {
        public ThrottlePolicy Policy { get; set; }
    }
}
