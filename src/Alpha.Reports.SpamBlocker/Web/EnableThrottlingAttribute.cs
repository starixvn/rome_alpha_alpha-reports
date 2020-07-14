namespace Alpha.Reports.SpamBlocker.Web
{
    using Alpha.Reports.SpamBlocker.Models;
    using System.Web.Mvc;

    public class EnableThrottlingAttribute : ActionFilterAttribute, IActionFilter
    {
        public ThrottlePolicy Policy { get; set; }
    }
}
