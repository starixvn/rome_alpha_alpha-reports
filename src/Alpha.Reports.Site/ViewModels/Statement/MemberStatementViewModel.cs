using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Alpha.Reports.Extensions;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.Constants;
using Alpha.Reports.Site.Helpers;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Utils;

namespace Alpha.Reports.Site.ViewModels.Statement
{
    public class MemberStatementViewModel : StatementViewModel
    {
        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public MemberStatementViewModel(
            MemberStatementQuery message,
            IEnumerable<StatementModel> statements)
             : base(message, statements, message.MinDate, message.FromDate, message.ToDate, message.UserSelectedDateRangeOption, message.IsFilterVisible, message.PageSize, message.PageIndex)
        {
            ProductIds = message.ProductIds ?? "1,6,2";
        }

        public string ProductIds { get; set; }

        public string BetListLink => UrlHelper.MapExMainPath("~/_BetList/BetList.aspx");

        public string BuildBetListLink()
        {
            var url = UrlHelper.MapBetListPath("~/MemberBetList/Index");
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["FromDate"] = SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
                ["ToDate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["GetAll"] = true,
                ["ProductIds"] = ProductIds,
                ["UserSelectedDateRangeOption"] = UserSelectedDateRangeOptionConstant.SpecificDates
            };

            return UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public string BuildBetListRunningLink()
        {
            var url = UrlHelper.MapBetListPath("~/RunningBetList/Index");
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = CustId,
                ["CustName"] = CustName,
                ["FromDate"] = SystemTime.Today.AddDays(-7).ToShortDateWithSlash(),
                ["ToDate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["GetAll"] = true,
                ["ProductIds"] = ProductIds
            };

            return UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public bool IsNegative(decimal? cellValue) => cellValue.HasValue && cellValue.Value < 0;
    }
}