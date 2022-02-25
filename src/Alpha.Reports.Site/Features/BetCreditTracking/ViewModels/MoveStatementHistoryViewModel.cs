namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.MoveStatementHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.Helpers;
    using Alpha.Reports.Site.ViewModels.Components;
    using System;
    using System.Collections.Generic;

    public class MoveStatementHistoryViewModel
    {
        private const string DefaultBetListPath = "~/MoveStatementHistory/Index";

        public MoveStatementHistoryViewModel(ReportData<MoveStatementHistoryModel> reportData, MoveStatementHistoryQuery message)
        {
            ReportData = reportData;
            CustLevel = message.ViewerLevel.DisplayName;
            IsFilterVisible = message.IsFilterVisible;
            MemberUsername = message.MemberUsername;
            MinDate = message.MinDate;
            MaxDate = DateTime.MaxValue;
            FromDate = message.FromDate;
            ToDate = message.ToDate;
            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
            ViewDataInstructionMessage = string.Format(Resources.CanOnlyViewDataAfterTimeHistoryReport, message.MinDate.ToShortDateWithSlash());
            BetListPath = Helpers.UrlHelper.MapBetListPath(DefaultBetListPath);
        }

        public ReportData<MoveStatementHistoryModel> ReportData { get; }

        public string CustLevel { get; }

        public bool IsFilterVisible { get; }

        public string MemberUsername { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
        
        public int UserSelectedDateRangeOption { get; }

        public string ViewDataInstructionMessage { get; }

        public string BetListPath { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
          => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public string GetBetListLinkRouteValues(MoveStatementHistoryModel model)
        {
            var routeValues = new Dictionary<string, object>
            {
                ["MemberRefNo"] = model.MemberRefNo
            };

            return UrlHelper.AddQueryStringParamsFunc(BetListPath, routeValues);
        }
    }
}