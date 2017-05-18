using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Alpha.Reports.Extensions;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.Helpers;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Utils;

namespace Alpha.Reports.Site.ViewModels.Statement
{
    public class MemberStatementViewModel : StatementViewModel
    {
        private const string StatementProductIds = "1,6,2";

        [SuppressMessage(
           "StyleCop.CSharp.StylecopPlus.Rules",
           "SP2100:CodeLineMustNotBeLongerThan",
           Justification = "Separate Code Makes Duplicate Lines With Other Classes.")]
        public MemberStatementViewModel(
            MemberStatementQuery message,
            IEnumerable<StatementModel> statements)
             : base(message, statements, message.MinDate, message.FromDate, message.ToDate, message.UserSelectedDateRangeOption, message.IsFilterVisible, message.PageSize, message.PageIndex)
        {
        }

        public string BetListLink => UrlHelper.MapExMainPath("~/_BetList/BetList.aspx");

        public string BuildBetListLink() => BuildBetListLink(option: 4, fromDate: SystemTime.Today.AddDays(-1));

        public string BuildBetListRunningLink() => BuildBetListLink(option: 5, fromDate: SystemTime.Today.AddDays(-7));

        public string BuildBetListLink(int option, DateTime fromDate)
        {
            var url = UrlHelper.MapExMainPath("~/_Reports/BetList/OutstandingDetail.aspx");

            var routeValues = new Dictionary<string, object>
            {
                ["username"] = CustName,
                ["option"] = option,
                ["custid"] = CustId,
                ["fdate"] = fromDate.ToShortDateWithSlash(),
                ["tdate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["page"] = 1,
                ["chkgetall"] = "on",
                ["productIds"] = StatementProductIds,
                ["FilterPostback"] = "1"
            };

            return UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public bool IsNegative(decimal? cellValue) => cellValue.HasValue && cellValue.Value < 0;
    }
}