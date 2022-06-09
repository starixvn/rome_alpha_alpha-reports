namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Helpers;
    using Alpha.Reports.Site.ViewModels.Components;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MoveStatementHistoryViewModel
    {
        private const string DefaultBetListPath = "~/MoveStatementHistory/Index";
        private readonly DateTime ReleasedDateTime = new DateTime(2022, 5, 31);

        public MoveStatementHistoryViewModel(IEnumerable<MoveStatementHistoryByMemberModel> data, MoveStatementHistoryQuery message, bool isError = false, int totalRows = 0)
        {
            Data = data;
            CustLevel = message.ViewerLevel.DisplayName;
            IsFilterVisible = message.IsFilterVisible;
            Username = message.Username;
            MinDate = message.MinDate.CompareTo(ReleasedDateTime) > 0 ? message.MinDate : ReleasedDateTime;
            FromDate = message.FromDate;
            ToDate = message.ToDate;
            PagerViewModel = new PagerViewModel(message.PageIndex, message.PageSize, totalRows);

            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, MinDate.ToShortDateWithSlash());
            BetListPath = Helpers.UrlHelper.MapBetListPath(DefaultBetListPath);
            CanViewBetLists = !message.IsProtectedMode || !message.CustLevel.IsAgentLevel();

            ErrMsg = isError ? Resources.InvalidUsername : string.Empty;
        }

        public IEnumerable<MoveStatementHistoryByMemberModel> Data { get; }

        public string CustLevel { get; }

        public bool IsFilterVisible { get; }

        public string Username { get; }

        public DateTime MinDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
        
        public string ViewDataInstructionMessage { get; }

        public string BetListPath { get; }

        public bool CanViewBetLists { get; }

        public MonthPickerViewModel MonthPickerViewModel
          => new MonthPickerViewModel(MinDate, FromDate, ToDate);

        public PagerViewModel PagerViewModel { get; }

        public string ErrMsg { get; }

        public bool IsShowDuplicatedTicketMessage => Data != null && Data.Any(member => member.RefNoRow.Any(row => row.IsMultipleMoved == 1));

        public string GetBetListLinkRouteValues(int custId, string custName, long transId)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["CustId"] = custId,
                ["CustName"] = custName,
                ["TransId"] = transId
            };

            return UrlHelper.AddQueryStringParamsFunc(BetListPath, routeValues);
        }
    }
}