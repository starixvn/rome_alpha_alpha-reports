namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using Extensions;
    using MediatR;
    using Params;
    using RemotingRef;
    using Requests.MatchWinLossDetail;
    using Services;
    using ViewModels.MatchWinLossDetail;

    public class MatchWinLossDetailHandler :
        MatchWinLossDetailBaseHandler,
        IRequestHandler<MatchWinLossDetailQuery, MatchWinLossDetailReportViewModel>
    {
        public MatchWinLossDetailHandler(
            ProductService productService,
            LeagueService leagueService,
            MatchWinLossDetailService reportService,
            CacheRefService cacheRefService) :
            base(productService, leagueService, reportService, cacheRefService)
        {
        }

        public MatchWinLossDetailReportViewModel Handle(MatchWinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var matchWinLossDetailParam = CreateParameter(message);
            var reportData = reportService.GetMatchWinLossDetail(matchWinLossDetailParam);

            var noParlayMatchWinLossDetail = GetNoParlayMatchWinLossDetail(reportData);
            var noParlayWinLossTotal = BuildWinLossTotal(noParlayMatchWinLossDetail);

            var products = reportService.GetProductsForCustomer(matchWinLossDetailParam);
            var leagues = leagueService.GetLeaguesByCustomer(new GetLeaguesByCustomerParam(
                message.CustId,
                matchWinLossDetailParam.UserSelectedProductIds.JoinWithComma(),
                message.FromDate,
                message.ToDate,
                message.Language));

            var mixParlay = GetMixParlay(reportData);
            var systemParlay = GetSystemParlay(reportData);

            return new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail,
                noParlayWinLossTotal,
                mixParlay,
                systemParlay,
                BuildWinLossGrandTotal(noParlayWinLossTotal, mixParlay, systemParlay),
                message,
                matchWinLossDetailParam,
                BuildProductSelectListItems(matchWinLossDetailParam.UserSelectedProductIds, products),
                BuildLeagueSelectListItems(matchWinLossDetailParam.Leagues, leagues));
        }
    }
}