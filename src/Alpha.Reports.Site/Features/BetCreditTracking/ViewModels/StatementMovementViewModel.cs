namespace Alpha.Reports.Site.Features.BetCreditTracking.ViewModels
{
    using Alpha.Reports.Extensions;
    using Alpha.Reports.Features.BetCreditTracking.StatementMovement.Models;
    using Alpha.Reports.Models;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Features.BetCreditTracking.Requests;
    using Alpha.Reports.Site.ViewModels.Components;
    using System;

    public class StatementMovementViewModel
    {
        public StatementMovementViewModel(ReportData<StatementMovementModel> reportData, StatementMovementQuery message)
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
        }

        public ReportData<StatementMovementModel> ReportData { get; }

        public string CustLevel { get; }

        public bool IsFilterVisible { get; }

        public string MemberUsername { get; }

        public DateTime MinDate { get; }

        public DateTime MaxDate { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }
        
        public int UserSelectedDateRangeOption { get; }

        public string ViewDataInstructionMessage { get; }

        public DateRangePickerViewModel DateRangePickerViewModel
          => new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);
    }
}