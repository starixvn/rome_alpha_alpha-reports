namespace Alpha.Reports.Site.Features.EnteredResult
{
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Handlers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EnteredSportResultBaseHandler : BaseHandler
    {
        protected readonly WinLossService reportService;

        public EnteredSportResultBaseHandler(WinLossService reportService)
        {
            this.reportService = reportService;
        }

        protected static EnteredSportResult GetTotalEnteredSportYesterdayAndToday(IEnumerable<EnteredSportResult> reportData)
        {
            if (reportData == null || !reportData.Any())
            {
                return null;
            }

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

            var totalToday = new DailyEnteredSport(totalEnteredToday, totalNonEnteredToday);
            var totalYesterday = new DailyEnteredSport(totalEnteredYesterday, totalNonEnteredYesterday);

            return new EnteredSportResult(0, Resources.Total, totalToday, totalYesterday);
        }
    }
}