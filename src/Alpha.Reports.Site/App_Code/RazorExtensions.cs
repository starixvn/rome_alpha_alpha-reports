namespace Alpha.Reports.Site
{
    using System;
    using System.Collections.Generic;
    using System.Web.WebPages;
    public static class RazorExtensions
    {
        public static HelperResult List<T>(this List<T> items, Func<T, HelperResult> template)
        {
            return new HelperResult(writer =>
            {
                foreach (var item in items)
                {
                    template(item).WriteTo(writer);
                }
            });
        }
    }
}