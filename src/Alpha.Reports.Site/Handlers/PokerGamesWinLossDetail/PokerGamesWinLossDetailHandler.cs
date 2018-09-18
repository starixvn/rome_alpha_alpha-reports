namespace Alpha.Reports.Site.Handlers.PokerGamesWinLossDetail
{
    using System;
    using System.Collections.Generic;
    using App_GlobalResources;
    using MediatR;
    using Reports.Models;
    using Requests.PokerGamesWinLossDetail;
    using Services;
    using ViewModels.PokerGamesWinLossDetail;

    public class PokerGamesWinLossDetailHandler :
        PokerGamesWinLossDetailBaseHandler,
        IRequestHandler<PokerGamesWinLossDetailQuery, PokerGamesWinLossDetailViewModel>,
        IRequestHandler<PokerGamesReportLegendQuery, IEnumerable<string>>
    {
        private readonly IUser viewer;

        public PokerGamesWinLossDetailHandler(PokerGamesWinLossDetailService reportService, IUser viewer) : base(reportService)
        {
            this.viewer = viewer;
        }

        public PokerGamesWinLossDetailViewModel Handle(PokerGamesWinLossDetailQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var winLossDetailParam = CreateParameter(message, message.CustLevel);

            var reportData = reportService.GetWinLossDetail(winLossDetailParam);

            var viewModel = new PokerGamesWinLossDetailViewModel(
                message,
                winLossDetailParam,
                reportData,
                BuildBreadcrumbLevel(message));

            return viewModel;
        }

        public IEnumerable<string> Handle(PokerGamesReportLegendQuery message)
        {
            message.CustId = viewer.Id;
            var result = new List<string>();
            result.AddRange(Resources.Tooltip_NetTurnover_PokerGames.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries));

            return result;
        }
    }
}