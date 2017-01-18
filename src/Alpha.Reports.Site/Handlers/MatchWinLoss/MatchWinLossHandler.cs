namespace Alpha.Reports.Site.Handlers.MatchWinLoss
{
    using MediatR;
    using Params;
    using RemotingRef;
    using Requests.MatchWinLoss;
    using Services;
    using ViewModels.MatchWinLoss;

    public class MatchWinLossHandler :
        MatchWinLossBaseHandler,
        IRequestHandler<MatchWinLossQuery, MatchWinLossReportViewModel>
    {
        private readonly ReportDateService reportDateService;

        public MatchWinLossHandler(
            MatchWinLossService reportService,
            CacheRefService cacheRefService,
            ReportDateService reportDateService) : base(reportService, cacheRefService)
        {
            this.reportDateService = reportDateService;
        }

        public MatchWinLossReportViewModel Handle(MatchWinLossQuery message)
        {
            var matchWinLossParam = new MatchWinLossParam(message.CustId, message.FromDate, message.ToDate);
            var rawData = reportService.GetMatchWinLoss(matchWinLossParam);
            var reportData = ConvertToViewModel(rawData);
            var viewModel = new MatchWinLossReportViewModel(
                message,
                minDate: reportDateService.GetBalanceUpDay(),
                matchWinLossParam: matchWinLossParam,
                reportData: reportData,
                grandTotal: AccumulateGrandTotal(reportData));

            return viewModel;
        }
    }
}