using System;
using System.Collections.Generic;
using Alpha.Reports.Site.Helpers;
using NSubstitute;

namespace Alpha.Reports.Site.Tests.Helpers
{
    public static class UrlHelperTestSetUp
    {
        public static void SetUp()
        {
            UrlHelper.MapExMainPath = Substitute.For<Func<string, string>>();
            UrlHelper.MapExMainPath(Arg.Any<string>()).Returns("BetListPath");

            UrlHelper.AddQueryStringParamsFunc = Substitute.For<Func<string, IDictionary<string, object>, string>>();
        }
    }
}
