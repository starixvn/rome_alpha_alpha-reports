namespace Alpha.Reports.Site.Handlers.MatchWinLossDetail
{
    using System;
    using System.Linq;
    using Alpha.Reports.Site.ViewModels.Components;
    using Extensions;
    using MediatR;
    using Params;
    using Reports.Models;
    using Requests.MatchWinLossDetail;
    using Services;
    using ViewModels.MatchWinLossDetail;

    public class MatchWinLossDetailHandler :
        MatchWinLossDetailBaseHandler,
        IRequestHandler<ReportLegendQuery, ReportLegendViewModel>,
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

            var products = reportService.GetProductsForCustomer(message.Site, message.CurrencyId);
            var matchWinLossDetailParam = CreateParameter(message, products);
            var reportData = reportService.GetMatchWinLossDetail(matchWinLossDetailParam);

            var noParlayMatchWinLossDetail = GetNoParlayMatchWinLossDetail(reportData);
            var noParlayWinLossTotal = BuildWinLossTotal(noParlayMatchWinLossDetail);

            var mixParlay = GetMixParlay(reportData);
            var systemParlay = GetSystemParlay(reportData);
            var parlayMatchWinLossDetail = GetParlayMatchWinLossDetail(reportData);

            var selectLeagueParam = new GetLeaguesByCustomerParam(
                message.CustId,
                matchWinLossDetailParam.UserSelectedProductIds.JoinWithComma(),
                message.FromDate,
                message.ToDate,
                message.LanguageKey);
            var leagues = string.IsNullOrWhiteSpace(message.UserSelectedLeagueIds) && !message.IsUserSelectsAllLeagues
                ? Enumerable.Empty<League>()
                : leagueService.GetLeaguesByCustomer(selectLeagueParam);

            var userSelectedLeagueIds = string.IsNullOrWhiteSpace(message.UserSelectedLeagueIds)
                ? leagues.Select(league => league.Id)
                : message.UserSelectedLeagueIds.Split(',').Select(id => id);

            var viewModel=  new MatchWinLossDetailReportViewModel(
                noParlayMatchWinLossDetail,
                noParlayWinLossTotal,
                mixParlay,
                systemParlay,
                parlayMatchWinLossDetail,
                BuildWinLossGrandTotal(noParlayWinLossTotal, mixParlay, systemParlay, parlayMatchWinLossDetail.ToList()),
                message,
                matchWinLossDetailParam,
                BuildProductSelectListItems(matchWinLossDetailParam.UserSelectedProductIds, products),
                BuildLeagueSelectListItems(userSelectedLeagueIds, leagues));
            return viewModel;
        }

        public ReportLegendViewModel Handle(ReportLegendQuery message)
            => new ReportLegendViewModel(Enumerable.Empty<string>(), GetGrossCommNotesByUserLevel(message.ViewerLevel), Enumerable.Empty<string>());
    }
}