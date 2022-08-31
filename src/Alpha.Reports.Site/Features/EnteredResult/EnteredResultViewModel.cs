namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using System;
    using System.Collections.Generic;
    using Utils;

    public class EnteredResultViewModel
    {
        public EnteredResultViewModel(
            EnteredSportResultQuery message,
            IEnumerable<EnteredSportResult> reportData,
            EnteredSportResult totalEnteredSport,
            bool isMobile = false)
        {
            ReportData = reportData;
            CustLevel = message.CustLevel;
            TotalEnteredSport = totalEnteredSport;
            IsMobileView = isMobile;
        }

        public DateTime MinDate => SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => SystemTime.Today;

        public DateTime ToDate => SystemTime.Today;

        public string Language { get; }

        public int UserSelectedDateRangeOption => 0;

        public IEnumerable<EnteredSportResult> ReportData { get; }

        public EnteredSportResult TotalEnteredSport { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public EnteredResultFilterViewModel FilterViewModel =>
            new EnteredResultFilterViewModel(
                CustLevel,
                DateRangePickerViewModel,
                MinDate);

        public UserLevel CustLevel { get; }

        public string Title
        {
            get
            {
                string v = CustLevel.IsMasterLevel()
                            ? Resources.AgentWinLoss
                            : Resources.MemberWinLoss;
                return CustLevel.IsSuperLevel()
                    ? Resources.MasterWinLoss
                    : v;
            }
        }

        public bool IsMobileView { get; }
    }
}