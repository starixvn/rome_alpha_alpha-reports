namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;
    using Reports.Models.Winloss;
    using Requests.WinLoss;
    using Utils;

    public class EnteredResultViewModel
    {
        public EnteredResultViewModel(
            EnteredSportResultQuery message,
            IEnumerable<EnteredSportResult> reportData)
        {
            ReportData = reportData;
            CustLevel = message.CustLevel;
        }

        public DateTime MinDate => SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => SystemTime.Today;

        public DateTime ToDate => SystemTime.Today;

        public string Language { get; }

        public int UserSelectedDateRangeOption => 0;

        public IEnumerable<EnteredSportResult> ReportData { get; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public EnteredResultFilterViewModel FilterViewModel =>
            new EnteredResultFilterViewModel(
                CustLevel,
                DateRangePickerViewModel,
                MinDate);

        public UserLevel CustLevel { get; }

        public string Title
            => CustLevel.IsSuperLevel()
            ? Resources.MasterWinLoss
            : CustLevel.IsMasterLevel()
            ? Resources.AgentWinLoss
            : Resources.MemberWinLoss;
    }
}