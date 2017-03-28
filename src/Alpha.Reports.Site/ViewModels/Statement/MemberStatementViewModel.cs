using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.Reports.Constants;
using Alpha.Reports.Extensions;
using Alpha.Reports.Models;
using Alpha.Reports.Models.Statement;
using Alpha.Reports.Site.App_GlobalResources;
using Alpha.Reports.Site.Helpers;
using Alpha.Reports.Site.Requests.Statement;
using Alpha.Reports.Site.ViewModels.Components;
using Alpha.Reports.Utils;

namespace Alpha.Reports.Site.ViewModels.Statement
{
    public class MemberStatementViewModel
    {
        private const string StatementProductIds = "1,6,2";

        private readonly MemberStatementQuery message;

        public MemberStatementViewModel(
            MemberStatementQuery message,
            IEnumerable<StatementModel> statements)
        {
            this.message = message;
            Statements = statements;

            PageSize = message.PageSize == 0 ? 50 : message.PageSize;
            PageIndex = message.PageIndex == 0 ? DefaultVariables.PageIndex : message.PageIndex;

            ViewDataInstructionMessage = string.Format(
                Resources.CanOnlyViewDataAfterTimeHistoryReport,
                message.MinDate.ToShortDateWithSlash());
        }

        public int CustId => message.CustId;

        public string CustName => message.CustName;

        public DateTime MinDate => message.MinDate;

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => message.FromDate;

        public DateTime ToDate => message.ToDate;

        public int UserSelectedDateRangeOption => message.UserSelectedDateRangeOption;

        public bool IsFilterVisible => message.IsFilterVisible;

        public int PageSize { get; }

        public int PageIndex { get; }

        public string ViewDataInstructionMessage { get; }

        public string BetListLink => UrlHelper.MapExMainPath("~/_BetList/BetList.aspx");

        public UserLevel CustLevel => message.CustLevel;

        public IEnumerable<StatementModel> Statements { get; }

        public DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public PagerViewModel PagerViewModel =>
            new PagerViewModel(PageIndex, PageSize, Statements.Count());

        public string BuildBetListLink()
        {
            var url = UrlHelper.MapExMainPath("~/_Reports/BetList/OutstandingDetail.aspx");
            var routeValues = new Dictionary<string, object>
            {
                ["username"] = CustName,
                ["option"] = 4,
                ["custid"] = CustId,
                ["fdate"] = SystemTime.Today.AddDays(-1).ToShortDateWithSlash(),
                ["tdate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["page"] = 1,
                ["chkgetall"] = "on",
                ["productIds"] = StatementProductIds,
                ["FilterPostback"] = "1"
            };

            return UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public string BuildBetListRunningLink()
        {
            var url = UrlHelper.MapExMainPath("~/_Reports/BetList/OutstandingDetail.aspx");
            var routeValues = new Dictionary<string, object>
            {
                ["username"] = CustName,
                ["option"] = 5,
                ["custid"] = CustId,
                ["fdate"] = SystemTime.Today.AddDays(-7).ToShortDateWithSlash(),
                ["tdate"] = SystemTime.Today.ToShortDateWithSlash(),
                ["page"] = 1,
                ["chkgetall"] = "on",
                ["productIds"] = StatementProductIds,
                ["FilterPostback"] = "1"
            };

            return UrlHelper.AddQueryStringParamsFunc(url, routeValues);
        }

        public bool IsNegative(decimal? cellValue)
        {
            return cellValue.HasValue && cellValue.Value < 0;
        }
    }
}