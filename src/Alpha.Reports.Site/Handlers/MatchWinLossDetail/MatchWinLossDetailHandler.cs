namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using System.Linq;
    using Extensions;
    using Fanex.Common;
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

            var products = reportService.GetProductsForCustomer(message.CustId);
            var matchWinLossDetailParam = CreateParameter(message, products);
            var reportData = reportService.GetMatchWinLossDetail(matchWinLossDetailParam);

            var noParlayMatchWinLossDetail = GetNoParlayMatchWinLossDetail(reportData);
            var noParlayWinLossTotal = BuildWinLossTotal(noParlayMatchWinLossDetail);

            var mixParlay = GetMixParlay(reportData);
            var systemParlay = GetSystemParlay(reportData);

            var selectLeagueParam = new GetLeaguesByCustomerParam(
                message.CustId,
                matchWinLossDetailParam.UserSelectedProductIds.JoinWithComma(),
                message.FromDate,
                message.ToDate,
                message.LanguageKey);
            var leagues = leagueService.GetLeaguesByCustomer(selectLeagueParam);

            var userSelectedLeagueIds = string.IsNullOrWhiteSpace(message.UserSelectedLeagueIds)
                ? leagues.Select(league => league.Id)
                : message.UserSelectedLeagueIds.Split(',').Select(id => Converter.ToInt(id));

            return new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail,
                noParlayWinLossTotal,
                mixParlay,
                systemParlay,
                BuildWinLossGrandTotal(noParlayWinLossTotal, mixParlay, systemParlay),
                message,
                matchWinLossDetailParam,
                BuildProductSelectListItems(matchWinLossDetailParam.UserSelectedProductIds, products),
                BuildLeagueSelectListItems(userSelectedLeagueIds, leagues));
        }
    }
}