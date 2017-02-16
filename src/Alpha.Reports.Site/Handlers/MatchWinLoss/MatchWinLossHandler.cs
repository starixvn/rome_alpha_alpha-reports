namespace Alpha.Reports.Site.Handlers.MatchWinLoss
{
    using System;
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
        public MatchWinLossHandler(MatchWinLossService reportService, CacheRefService cacheRefService)
            : base(reportService, cacheRefService)
        {
        }

        public MatchWinLossReportViewModel Handle(MatchWinLossQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var matchWinLossParam = new MatchWinLossParam(message.CustId, message.FromDate, message.ToDate);
            var rawData = reportService.GetMatchWinLoss(matchWinLossParam);
            var reportData = BuildViewModel(rawData);
            var viewModel = new MatchWinLossReportViewModel(
                message,
                minDate: message.MinDate,
                matchWinLossParam: matchWinLossParam,
                reportData: reportData,
                grandTotal: AccumulateGrandTotal(reportData));

            return viewModel;
        }
    }
}