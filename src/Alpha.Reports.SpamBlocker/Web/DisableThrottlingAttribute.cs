namespace Alpha.Reports.SpamBlocker.Web
{
    using System;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public class DisableThrottlingAttribute : ActionFilterAttribute
    {
    }
}
