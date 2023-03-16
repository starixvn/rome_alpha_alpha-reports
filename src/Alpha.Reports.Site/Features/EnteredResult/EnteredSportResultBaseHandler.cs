namespace Alpha.Reports.Site.Features.EnteredResult
{
    using System.Collections.Generic;
    using System.Linq;
    using Alpha.Reports.Models.Winloss;
    using Alpha.Reports.Services;
    using Alpha.Reports.Site.App_GlobalResources;
    using Alpha.Reports.Site.Handlers;

    public class EnteredSportResultBaseHandler : BaseHandler
    {
        protected readonly WinLossService reportService;

        public EnteredSportResultBaseHandler(WinLossService reportService)
        {
            this.reportService = reportService;
        }

        protected EnteredSportResult GetTotalEnteredSportYesterdayAndToday(IEnumerable<EnteredSportResult> reportData)
        {
            if (reportData == null || !reportData.Any())
            {
                return null;
            }

            var result = reportService.GetTotalEnteredSportYesterdayAndToday(reportData);

            return new EnteredSportResult(0, Resources.Total, result.EnteredSportToday, result.EnteredSportYesterday);
        }
    }
}