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
    using System.Linq;
    using Utils;

    public class EnteredResultViewModel
    {
        public EnteredResultViewModel(
            EnteredSportResultQuery message,
            IEnumerable<EnteredSportResult> reportData,
            bool isMobile = false)
        {
            ReportData = reportData;
            CustLevel = message.CustLevel;
            IsMobileView = isMobile;

            var total = GetTotalEnteredSportYesterdayAndToday(reportData);
            TotalEnteredSportYesterday = total.Item1;
            TotalEnteredSportToday = total.Item2;
        }

        public DateTime MinDate => SystemTime.Today.AddMonths(-3).FirstDayOfMonth();

        public DateTime MaxDate => DateTime.MaxValue;

        public DateTime FromDate => SystemTime.Today;

        public DateTime ToDate => SystemTime.Today;

        public string Language { get; }

        public int UserSelectedDateRangeOption => 0;

        public IEnumerable<EnteredSportResult> ReportData { get; }

        public DailyEnteredSport TotalEnteredSportToday { get; }

        public DailyEnteredSport TotalEnteredSportYesterday { get; }

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

        private Tuple<DailyEnteredSport, DailyEnteredSport> GetTotalEnteredSportYesterdayAndToday(IEnumerable<EnteredSportResult> reportData)
        {
            int totalEnteredToday = 0,
                totalNonEnteredToday = 0,
                totalEnteredYesterday = 0,
                totalNonEnteredYesterday = 0;

            if (reportData.Any())
            {
                foreach (var data in reportData)
                {
                    if (data.EnteredSportToday != null)
                    {
                        totalEnteredToday += data.EnteredSportToday.Entered;
                        totalNonEnteredToday += data.EnteredSportToday.NonEntered;
                    }

                    if (data.EnteredSportYesterday != null)
                    {
                        totalEnteredYesterday += data.EnteredSportYesterday.Entered;
                        totalNonEnteredYesterday += data.EnteredSportYesterday.NonEntered;
                    }
                }
            }

            return Tuple.Create(
                new DailyEnteredSport(totalEnteredYesterday, totalNonEnteredYesterday),
                new DailyEnteredSport(totalEnteredToday, totalNonEnteredToday));
        }
    }
}