namespace Alpha.Reports.Site.ViewModels.WinLoss
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using Components;
    using Extensions;
    using Reports.Models;
    using Reports.Models.Winloss;

    public class EnteredResultViewModel
    {
        public DateTime MinDate => DateTime.Now.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Language { get; set; }

        public int UserSelectedDateRangeOption { get; set; }

        public IEnumerable<EnteredSportResult> ReportData { get; set; }

        private DateRangePickerViewModel DateRangePickerViewModel =>
            new DateRangePickerViewModel(MinDate, MaxDate, FromDate, ToDate, UserSelectedDateRangeOption);

        public EnteredResultFilterViewModel FilterViewModel =>
            new EnteredResultFilterViewModel(
                UserLevel,
                DateRangePickerViewModel,
                MinDate);

        public UserLevel UserLevel { get; set; }

        public string Title
        {
            get
            {
                if (UserLevel.IsSuperLevel())
                {
                    return Resources.MasterWinLoss;
                }

                if (UserLevel.IsMasterLevel())
                {
                    return Resources.AgentWinLoss;
                }

                return Resources.MemberWinLoss;
            }
        }
    }
}