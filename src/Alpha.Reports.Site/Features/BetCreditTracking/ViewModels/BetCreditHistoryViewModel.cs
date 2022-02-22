namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Features.BetCreditTracking.BetCreditHistory.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using System;

    public class BetCreditHistoryViewModel
    {
        public BetCreditHistoryViewModel(ReportData<BetCreditHistoryModel> reportData, BetCreditHistoryQuery message)
        {
            ReportData = reportData;
            CustLevel = message.ViewerLevel.DisplayName;
            Member = message.Member;
            RefNo = message.RefNo;
            IsFilterVisible = message.IsFilterVisible;
            MinDate = message.MinDate;
            MaxDate = DateTime.MaxValue;
            FromDate = message.FromDate;
            ToDate = message.ToDate;
            UserSelectedDateRangeOption = message.UserSelectedDateRangeOption;
        }

        public ReportData<BetCreditHistoryModel> ReportData { get; }

        public string CustLevel { get; }

        public string Member { get; }

        public string RefNo { get; }

        public bool IsFilterVisible { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public int UserSelectedDateRangeOption { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
         => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);
    }
}